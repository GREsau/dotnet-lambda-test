@IF "%1"=="" (set stack=graham-dotnet-lambda-test) else (set stack=%1)
aws cloudformation deploy --template-file packaged.yaml --stack-name %stack% --capabilities CAPABILITY_IAM
aws cloudformation describe-stacks --query "Stacks[0].Outputs" --stack-name %stack%