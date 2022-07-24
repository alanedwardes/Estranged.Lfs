# Estranged.Lfs ![Build status](https://github.com/alanedwardes/Estranged.Lfs/workflows/.NET%20Core/badge.svg)

A Git LFS backend which provides pluggable authentication and blob store adapters. It is designed to run in a serverless environment to be used in conjunction with a Git provider such as GitHub or BitBucket, or self hosted Git.

## Basic Usage

1. Add the Git LFS services to your application:

```csharp
services.AddLfs();
```

2. Register an implementation for IBlobAdapter and IAuthenticator. Amazon AWS S3 and Azure Blob Storage are provided out of the box:

```csharp
var s3BlobConfig = new S3BlobAdapterConfig
{
    Bucket = "estranged-lfs-test"
};
services.AddLfsS3Adapter(s3BlobConfig, new AmazonS3Client());
services.AddLfsDictionaryAuthenticator(new Dictionary<string, string>{{"username","password"}});
```

Or use the following example for Azure Blob Storage.

> Note: Keep the naming rules for Azure Blob Storage in account, review them [here](https://docs.microsoft.com/en-us/rest/api/storageservices/Naming-and-Referencing-Containers--Blobs--and-Metadata).

```csharp
var blobServiceClient = new Azure.Storage.Blobs.BlobServiceClient("<your connection string here>");
var blobConfig = new AzureBlobAdapterConfig
{
    ContainerName = "estranged-lfs-test"
};
services.AddLfsAzureBlobAdapter(blobConfig, blobServiceClient);
services.AddLfsDictionaryAuthenticator(new Dictionary<string, string> {{"username","password"}});
```

### GitHub Authenticator

A GitHub authenticator implementation is provided out of the box. This authenticator takes the supplied username and password and makes a "get repository" call against the GitHub API. If the result is that the user has access, the LFS call succeeds, if the user does not have access, the LFS call fails with a 401 error.

To configure the GitHub authenticator, you need to register it with the `IServiceProvider`:

```csharp
// services.AddLfsDictionaryAuthenticator(new Dictionary<string, string>{{"username","password"}});
var ghAuthConfig = new GitHubAuthenticatorConfig
{
    Organisation = "alanedwardes",
    Repository = "Estranged.Lfs"
};
services.AddLfsGitHubAuthenticator(ghAuthConfig);
```

When LFS prompts you for credentials, enter your GitHub username, and a [personal access token](https://github.com/settings/tokens) to authenticate. Your token should have the "repository read" scope.

### BitBucket Authenticator

A BitBucket authenticator implementation is provided out of the box. This authenticator takes the supplied username and password and makes a "get repository" call against the BitBucket API. If the result is that the user has access, the LFS call succeeds, if the user does not have access, the LFS call fails with a 401 error.

To configure the BitBucket authenticator, you need to register it with the `IServiceProvider`:

```csharp
// services.AddLfsDictionaryAuthenticator(new Dictionary<string, string>{{"username","password"}});
var bbAuthConfig = new BitBucketAuthenticatorConfig
{
    Workspace = "alanedwardes",
    Repository = "Estranged.Lfs"
};
services.AddLfsBitBucketAuthenticator(bbAuthConfig);
```

When LFS prompts you for credentials, enter your BitBucket username, and a [personal access token](https://bitbucket.org/account/settings/app-passwords/) to authenticate. Your token should have the "repository read" scope.

## Extensibility

### Blob Adapter

Any blob store which generates pre-signed URLs can be used by implementing the interface IBlobAdapter:

```csharp
public interface IBlobAdapter
{
    Task<SignedBlob> UriForUpload(string oid, long size, CancellationToken token);
    Task<SignedBlob> UriForDownload(string oid, CancellationToken token);
}
```

An S3 implementation is included, which generates pre-signed GET and PUT requests. This can be used out of the box if desired.

### Authentication Adapter

Git LFS supports HTTP Basic authentication, the mechanics of which the library deals with but the authentication portion is exposed behind the IAuthenticator interface.

```csharp
public interface IAuthenticator
{
    Task Authenticate(string username, string password, LfsPermission requiredPermission, CancellationToken token);
}
```

A sample implementation exposing a dictionary of username => password is included as a reference.

## Example Deployables

There are currently two hosting examples:

- `Estranged.Lfs.Hosting.AspNet`
- `Estranged.Lfs.Hosting.Lambda`

The former is a simple example using only Asp.NET components, and the latter is an Asp.NET Lambda function which can be deployed directly to AWS Lambda, behind API Gateway.

#### Deploying to Lambda

1. Head over to the `Estranged.Lfs.Hosting.Lambda` project in the `hosting` folder.
2. Install the `dotnet-lambda` global tool from AWS: https://github.com/aws/aws-extensions-for-dotnet-cli
3. Edit the `aws-lambda-tools-defaults.json` file to suit your environment setup:

```javascript
{
    "profile": "default",
    "configuration": "Release",
    "framework": "net6.0",
    "function-handler": "Estranged.Lfs.Hosting.Lambda::Estranged.Lfs.Hosting.Lambda.LambdaEntryPoint::FunctionHandlerAsync",
    "function-memory-size": 256,
    "function-timeout": 30,
    "function-runtime": "dotnet6",
    "region": "<aws region>",
    "s3-bucket": "<s3 bucket to upload the lambda to>",
    "s3-prefix": "<path in s3 to upload the lambda to>",
    "function-name": "<lambda name to deploy or update>",
    // Set other variables required by the Lambda function
    "environment-variables": "LFS_BUCKET=<lfs s3 bucket>;<key>=<value>"
}
```

4. Run `dotnet-lambda deploy-serverless` to deploy the Lambda function
