// --------------------------------------------------------------------------------------------------------------------
// <copyright file="GitHubClientService.cs" company="MareMare">
// Copyright © 2024 MareMare.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using Octokit;
using Spectre.Console;

namespace GitHubCommit.Services;

/// <summary>
/// GitHubと対話し、ログインやリポジトリ内のファイルの作成・更新を行うサービスを提供します。
/// </summary>
#pragma warning disable CA1812
internal sealed class GitHubClientService(IAnsiConsole console) : IGitHubClientService
#pragma warning restore CA1812
{
/// <summary>
/// コンソール出力を行うためのインターフェース。
/// </summary>
    private readonly IAnsiConsole _console = console;

    /// <summary>
    /// GitHubクライアントのインスタンス。
    /// </summary>
    private GitHubClient? _client;

    /// <inheritdoc />
    public async ValueTask LoginAsync(string? token = null)
    {
        token ??= Environment.GetEnvironmentVariable("GH_TOKEN");
        try
        {
            this._console.MarkupLine("Logging in to GitHub.");
            var tokenAuth = new Credentials(token);
            var client = new GitHubClient(new ProductHeaderValue("GitHubCommit")) { Credentials = tokenAuth };

            var user = await client.User.Current().ConfigureAwait(false);
            this._console.MarkupLine($"Logged in to GitHub [green]as {user.Login}[/].");
            this._client = client;
        }
        catch (Exception ex)
        {
            this._client = null;
            this._console.MarkupLine("[red]Fail to login to GitHub.[/]");
            this._console.WriteException(ex);
            throw; // Use throw; to preserve stack trace
        }
    }

    /// <inheritdoc />
    public async ValueTask CreateOrUpdateFileAsync(
        string owner,
        string repo,
        string branch,
        string targetFilePath,
        string contentBase64String,
        string? message)
    {
        if (this._client is null)
        {
            throw new InvalidOperationException("GitHub client is not initialized.");
        }

        var sha = await this.FindShaAsync(this._client, owner, repo, branch, targetFilePath)
            .ConfigureAwait(false);
        if (string.IsNullOrEmpty(sha))
        {
            await this.CreateFileAsync(owner, repo, branch, targetFilePath, contentBase64String, message)
                .ConfigureAwait(false);
        }
        else
        {
            await this.UpdateFileAsync(owner, repo, branch, targetFilePath, contentBase64String, sha, message)
                .ConfigureAwait(false);
        }
    }

    /// <summary>
    /// 指定されたGitHubリポジトリに新しいファイルを作成します。
    /// </summary>
    /// <param name="owner">リポジトリの所有者。</param>
    /// <param name="repo">リポジトリの名前。</param>
    /// <param name="branch">ファイルを作成するブランチ。</param>
    /// <param name="targetFilePath">作成するファイルのパス。</param>
    /// <param name="contentBase64String">ファイルのBase64エンコードされた内容。</param>
    /// <param name="message">ファイル作成のコミットメッセージ。</param>
    /// <returns>非同期操作を表すタスク。</returns>
    private async Task CreateFileAsync(
        string owner,
        string repo,
        string branch,
        string targetFilePath,
        string contentBase64String,
        string? message)
    {
        var resolvedMessage = string.IsNullOrEmpty(message) ? $"Create {targetFilePath}." : message;

        var request = new CreateFileRequest(
            resolvedMessage,
            contentBase64String,
            branch,
            convertContentToBase64: false);
        await this._client!.Repository.Content
            .CreateFile(owner, repo, targetFilePath, request)
            .ConfigureAwait(false);
        this._console.MarkupLine(
            $"[green]File Created:[/] {owner}/{repo} on {branch}, Path: [yellow]{targetFilePath}[/]");
    }

    /// <summary>
    /// 指定されたGitHubリポジトリの既存のファイルを更新します。
    /// </summary>
    /// <param name="owner">リポジトリの所有者。</param>
    /// <param name="repo">リポジトリの名前。</param>
    /// <param name="branch">ファイルを更新するブランチ。</param>
    /// <param name="targetFilePath">更新するファイルのパス。</param>
    /// <param name="contentBase64String">ファイルのBase64エンコードされた内容。</param>
    /// <param name="sha">更新するファイルのSHA。</param>
    /// <param name="message">ファイル更新のコミットメッセージ。</param>
    /// <returns>非同期操作を表すタスク。</returns>
    private async Task UpdateFileAsync(
        string owner,
        string repo,
        string branch,
        string targetFilePath,
        string contentBase64String,
        string sha,
        string? message)
    {
        var resolvedMessage = string.IsNullOrEmpty(message) ? $"Update {targetFilePath}." : message;

        var request = new UpdateFileRequest(
            resolvedMessage,
            contentBase64String,
            sha,
            branch,
            convertContentToBase64: false);
        await this._client!.Repository.Content
            .UpdateFile(owner, repo, targetFilePath, request)
            .ConfigureAwait(false);
        this._console.MarkupLine(
            $"[green]File Updated:[/] {owner}/{repo} on {branch}, Path: [yellow]{targetFilePath}[/]");
    }

    /// <summary>
    /// 指定されたGitHubリポジトリ内のファイルのSHAを検索します。
    /// </summary>
    /// <param name="client">GitHubクライアント。</param>
    /// <param name="owner">リポジトリの所有者。</param>
    /// <param name="repo">リポジトリの名前。</param>
    /// <param name="branch">ファイルが存在するブランチ。</param>
    /// <param name="targetFilePath">検索するファイルのパス。</param>
    /// <returns>非同期操作を表すタスク。タスクの結果はファイルのSHA、またはファイルが存在しない場合はnullです。</returns>
    private async ValueTask<string?> FindShaAsync(
        GitHubClient client,
        string owner,
        string repo,
        string branch,
        string targetFilePath)
    {
        try
        {
            var existingFile = await client.Repository.Content
                .GetAllContentsByRef(owner, repo, targetFilePath, branch)
                .ConfigureAwait(false);
            return existingFile?.ElementAtOrDefault(0)?.Sha;
        }
        catch (NotFoundException)
        {
            this._console.MarkupLine($"[red]Not found file.[/] Path: [yellow]{targetFilePath}[/]");
            return null;
        }
    }
}
