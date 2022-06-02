# Poccy

## Running
- Linux
    ```
        dotnet run -r linux-x64
    ```

## Development
1. DotNet Installation
    - Instructions (Ubuntu 20.04)
        - wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
        - 
            ```bash
                wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
                sudo dpkg -i packages-microsoft-prod.deb
                rm packages-microsoft-prod.deb
            ```
        - 
            ```
                sudo apt-get update; \
                    sudo apt-get install -y apt-transport-https && \
                    sudo apt-get update && \
                    sudo apt-get install -y dotnet-sdk-6.0
            ```
    - Notes
        - Currently using DotNet 6
        - Linux Install Docs: https://docs.microsoft.com/en-us/dotnet/core/install/linux