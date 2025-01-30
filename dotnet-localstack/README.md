# .NET with LocalStack Dev Container

Development container for .NET 8 API development with LocalStack for AWS SQS integration.

This dev container shows and example of using Docker from Docker. The dev container has access to the host machine's Docker socket and can run Docker commands directly. This allows it to start the LocalStack container and run commands to create the queue and send messages.

## Features

- .NET 8 SDK with API development tools
- LocalStack for AWS service emulation
- AWS CLI configured for LocalStack
- Just command runner for common tasks
- zsh with Oh My Zsh and completions

## Quick Start

1. Open in VS Code and click "Reopen in Container"
2. Start LocalStack and create queue:
   ```bash
   just setup
   ```
3. Test the queue:
   ```bash
   just send-message    # Send a test message
   just list-queues     # List available queues
   just receive-messages # Receive messages
   ```

## Available Commands

Run `just` to see all available commands. Key commands:

- `just setup` - Start LocalStack, create queue, and run API
- `just start-localstack` - Start LocalStack container
- `just stop-localstack` - Stop LocalStack container
- `just create-queue` - Create test SQS queue
- `just send-message` - Send test message
- `just receive-messages` - Receive messages from queue

## Container Details

- Ports:
  - 5118: .NET API
  - 4566: LocalStack
- Environment:
  - AWS credentials configured for LocalStack
  - AWS CLI paging disabled
  - Host network mode for container communication
