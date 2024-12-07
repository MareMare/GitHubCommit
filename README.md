# GitHubCommit

It's simply a tool to commit to GitHub.

GitHubCommit is a simple command-line tool designed to facilitate committing changes to GitHub repositories. It streamlines the process by providing straightforward commands and automating common tasks associated with Git commits.

## Requirements

- [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is required to run this tool.
- Ensure that the environment variable `GH_TOKEN` is set. This token is required for authenticating with GitHub to commit changes.

## Installation

1. Download the latest release from the [GitHub releases page](https://github.com/MareMare/GitHubCommit/releases).
2. Ensure that [.NET Desktop Runtime 8.0](https://dotnet.microsoft.com/en-us/download/dotnet/8.0) is installed on your machine.
3. Extract the downloaded files to your preferred directory.

## Usage

```ps1
.\GitHubCommit.exe <SourceFilePath> <TargetFilePath> [OPTIONS]
```

### Arguments

| Argument       | Description                                                        |
|----------------|--------------------------------------------------------------------|
| SourceFilePath | Specify the local path of the file to commit                       |
| TargetFilePath | Specify the path of the file to create or update in the repository |

### Options

| Option                 | Default     | Description                                                      |
|------------------------|-------------|------------------------------------------------------------------|
| `-h, --help`           |             | Prints help information                                          |
| `--repo <OWNERREPO>`   | OWNER/REPO  | Select another repository using the OWNER/REPO format            |
| `--branch <BRANCH>`    | main        | Specify the branch where the file will be created or updated     |
| `--message <MESSAGE>`  |             | Specify the commit message for the file creation or update       |

## Examples

Example of committing changes to the main branch:
```ps1
.\GitHubCommit.exe sampling_2024-12.csv data/sampling.csv --repo MareMare/GHCommitSandbox
```
