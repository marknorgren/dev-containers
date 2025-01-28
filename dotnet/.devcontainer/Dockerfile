FROM mcr.microsoft.com/devcontainers/dotnet:8.0-noble

# Avoid prompts during package installation
ENV DEBIAN_FRONTEND=noninteractive

# Create the user
ARG USERNAME=vscode
ARG USER_UID=2000
ARG USER_GID=2000

# # Create the user and group
# RUN if getent group $USER_GID; then \
# 	echo "Group $USER_GID exists, skipping group creation"; \
# 	else \
# 	groupadd --gid $USER_GID $USERNAME; \
# 	fi \
# 	&& useradd --uid $USER_UID --gid $USER_GID -m $USERNAME 2>/dev/null || useradd --uid $USER_UID -g $USER_GID -m $USERNAME \
# 	# Add sudo support
# 	&& apt-get update \
# 	&& apt-get install -y sudo \
# 	&& echo $USERNAME ALL=\(root\) NOPASSWD:ALL > /etc/sudoers.d/$USERNAME \
# 	&& chmod 0440 /etc/sudoers.d/$USERNAME

# Install required packages
RUN apt-get update && apt-get install -y \
	curl \
	unzip \
	just \
	&& rm -rf /var/lib/apt/lists/*

# Install AWS CLI v2
RUN curl "https://awscli.amazonaws.com/awscli-exe-linux-aarch64.zip" -o "awscliv2.zip" \
	&& unzip awscliv2.zip \
	&& ./aws/install \
	&& rm -rf aws awscliv2.zip

# Create VS Code directories with proper permissions
RUN mkdir -p /home/$USERNAME/.vscode-server/extensions \
	/home/$USERNAME/.vscode-server-insiders/extensions \
	&& chown -R $USERNAME:$USERNAME /home/$USERNAME/.vscode-server \
	&& chown -R $USERNAME:$USERNAME /home/$USERNAME/.vscode-server-insiders

# Set the default user
USER $USERNAME

# Set the default working directory
WORKDIR /workspace
