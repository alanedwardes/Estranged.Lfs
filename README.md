# Estranged.Lfs [![Build status](https://ci.appveyor.com/api/projects/status/q5rkhprtqviyurlx?svg=true)](https://ci.appveyor.com/project/alanedwardes/estranged-lfs)
A Git LFS backend which provides pluggable authentication and blob store adapters. It is designed to run in a serverless environment to be used in conjunction with a Git provider such as GitHub or BitBucket, or self hosted Git.

## Basic Usage
1. Add the Git LFS services to your application:
```csharp
services.AddLfs();
```
2. Register an implementation for IBlobAdapter and IAuthenticator. S3 is provided out of the box:
```csharp
services.AddSingleton<IBlobAdapter, S3BlobAdapter>();
services.AddSingleton<IAuthenticator>(x => new DictionaryAuthenticator(new Dictionary<string, string> { { "username", "password" } }));

// Required when using S3BlobAdapter
services.AddSingleton<IS3BlobAdapterConfig>(x => new S3BlobAdapterConfig { Bucket = "estranged-lfs-test" });
services.AddSingleton<IAmazonS3>(x => new AmazonS3Client());
```
To use another blob store or authentication provider, register your own implementations into the services container. See below for more details.

## Extensibility

### Blob Adapter
Any blob store which generates pre-signed URLs can be used by implementing the interface IBlobAdapter:

```csharp
public interface IBlobAdapter
{
    Task<SignedBlob> UriForUpload(string Oid, long size);
    Task<SignedBlob> UriForDownload(string Oid);
}
```
An S3 implementation is included, which generates pre-signed GET and PUT requests. This can be used out of the box if desired.

### Authentication Adapter
Git LFS supports HTTP Basic authentication, the mechanics of which the library deals with but the authentication portion is exposed behind the IAuthenticator interface.

```csharp
public interface IAuthenticator
{
    bool Authenticate(string username, string password);
}
```
A sample implementation exposing a dictionary of username => password is included as a reference.

## Example Deployables
There are currently two hosting examples:
* Estranged.Lfs.Hosting.AspNet
* Estranged.Lfs.Hosting.Lambda

The former is a simple example using only Asp.NET components, and the latter is an Asp.NET Lambda function which can be deployed directly to AWS Lambda, behind API Gateway.
