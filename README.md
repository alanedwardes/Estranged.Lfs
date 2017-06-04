# Estranged.Lfs
A Git LFS backend which provides pluggable authentication and blob store adapters. It is designed to run in a serverless environment to be used in conjunction with a Git provider such as GitHub or BitBucket, or self hosted Git.

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

## Getting Started
There are currently two hosting examples:
* Estranged.Lfs.Hosting.AspNet
* Estranged.Lfs.Hosting.Lambda

The former is a simple example using only Asp.NET components, and the latter is an Asp.NET Lambda function which can be deployed directly to AWS Lambda, behind API Gateway.
