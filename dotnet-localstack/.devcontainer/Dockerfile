FROM mcr.microsoft.com/devcontainers/dotnet:8.0-noble

# Install additional packages
RUN apt-get update && export DEBIAN_FRONTEND=noninteractive \
	&& apt-get -y install --no-install-recommends \
	just \
	curl \
	unzip \
	docker.io \
	zsh \
	git \
	zsh-autosuggestions \
	zsh-syntax-highlighting \
	&& apt-get clean \
	&& rm -rf /var/lib/apt/lists/*

# Update .NET workloads
RUN dotnet workload update

# Setup zsh for vscode user
USER vscode
WORKDIR /home/vscode

# Configure zsh with plugins and settings
RUN echo '# Custom configurations' >> ~/.zshrc && \
	echo 'DISABLE_AUTO_UPDATE="true"' >> ~/.zshrc && \
	echo 'DISABLE_UPDATE_PROMPT="true"' >> ~/.zshrc && \
	echo '' >> ~/.zshrc && \
	echo '# Just completion' >> ~/.zshrc && \
	mkdir -p ~/.zsh/completion && \
	just --completions zsh > ~/.zsh/completion/_just && \
	echo 'fpath=(~/.zsh/completion $fpath)' >> ~/.zshrc && \
	echo 'autoload -U compinit; compinit' >> ~/.zshrc && \
	echo '' >> ~/.zshrc && \
	echo '# Additional plugins' >> ~/.zshrc && \
	echo 'source /usr/share/zsh-autosuggestions/zsh-autosuggestions.zsh' >> ~/.zshrc && \
	echo 'source /usr/share/zsh-syntax-highlighting/zsh-syntax-highlighting.zsh' >> ~/.zshrc

# Set zsh as default shell for vscode user
USER root
RUN chsh -s /bin/zsh vscode

WORKDIR /

# Install AWS CLI based on architecture
RUN if [ "$(uname -m)" = "aarch64" ]; then \
	curl "https://awscli.amazonaws.com/awscli-exe-linux-aarch64.zip" -o "awscliv2.zip"; \
	else \
	curl "https://awscli.amazonaws.com/awscli-exe-linux-x86_64.zip" -o "awscliv2.zip"; \
	fi \
	&& unzip awscliv2.zip \
	&& ./aws/install \
	&& rm -rf aws awscliv2.zip

# Configure AWS CLI for LocalStack
RUN mkdir -p /root/.aws \
	&& echo "[default]\nregion=us-west-2\noutput=json\n" > /root/.aws/config \
	&& echo "[default]\naws_access_key_id=test\naws_secret_access_key=test" > /root/.aws/credentials

# Also configure AWS CLI for non-root user
RUN mkdir -p /home/vscode/.aws \
	&& echo "[default]\nregion=us-west-2\noutput=json\n" > /home/vscode/.aws/config \
	&& echo "[default]\naws_access_key_id=test\naws_secret_access_key=test" > /home/vscode/.aws/credentials \
	&& chown -R vscode:vscode /home/vscode/.aws
# Add vscode user to docker group
RUN groupadd docker || true \
	&& usermod -aG docker vscode

