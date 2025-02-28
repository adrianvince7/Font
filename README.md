# Font - Support for Recovery

A Blazor WebAssembly application built with .NET 8 and AWS services to provide support for individuals in recovery.

## Architecture

The application follows a microservices architecture using:
- Blazor WebAssembly for the frontend
- .NET Web APIs deployed as AWS Lambda functions
- AWS Cognito for authentication
- Amazon RDS (MySQL) for data storage
- AWS S3 for file storage
- CloudFront for content delivery

## Prerequisites

- .NET 8.0 SDK
- AWS Account with free tier access
- AWS CLI configured locally
- Git

## Local Development

1. Clone the repository:
```bash
git clone https://github.com/yourusername/Font.git
cd Font
```

2. Install dependencies:
```bash
dotnet restore
```

3. Update `appsettings.Development.json` with your AWS credentials and configuration.

4. Run the API:
```bash
cd src/Font.Api
dotnet run
```

5. Run the Client:
```bash
cd src/Font.Client
dotnet run
```

## Testing

Run the test suite:
```bash
dotnet test
```

## Deployment

The application uses GitHub Actions for CI/CD. To deploy:

1. Configure the following secrets in your GitHub repository:
   - AWS_ACCESS_KEY_ID
   - AWS_SECRET_ACCESS_KEY
   - CLOUDFRONT_DISTRIBUTION_ID

2. Push to the main branch to trigger the deployment pipeline.

## Infrastructure

Infrastructure is managed using AWS CloudFormation. The template is located in `infrastructure/cloudformation.yaml`.

To deploy the infrastructure manually:
```bash
aws cloudformation deploy \
  --template-file infrastructure/cloudformation.yaml \
  --stack-name font-app-stack \
  --parameter-overrides Environment=Production \
  --capabilities CAPABILITY_NAMED_IAM
```

## Contributing

1. Create a feature branch
2. Make your changes
3. Run tests
4. Submit a pull request

## License

MIT
