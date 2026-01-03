// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommitCommand.cs" company="MareMare">
// Copyright © 2024 MareMare.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using GitHubCommit.Services;
using Spectre.Console.Cli;

namespace GitHubCommit.Commands;

/// <summary>
/// GitHubと対話し、ログインやリポジトリ内のファイルの作成・更新を行うコマンドを表します。
/// </summary>
/// <param name="service"><see cref="IGitHubClientService" />。</param>
internal sealed class CommitCommand(IGitHubClientService service) : AsyncCommand<CommitCommandSettings>
{
    /// <inheritdoc />
    public override async Task<int> ExecuteAsync(CommandContext context, CommitCommandSettings settings, CancellationToken cancellationToken)
    {
        ArgumentNullException.ThrowIfNull(settings);

        await service.LoginAsync().ConfigureAwait(false);

        var (owner, repo) = SplitOwnerRepo(settings.OwnerRepo);
        var contentBase64String = FileEncoder.EncodeFileToBase64(settings.SourceFilePath);

        await service.CreateOrUpdateFileAsync(
                owner,
                repo,
                settings.Branch,
                settings.TargetFilePath,
                contentBase64String,
                settings.Message)
            .ConfigureAwait(false);

        return 0;

        static (string Owner, string Repo) SplitOwnerRepo(string ownerRepo)
        {
            var elements = ownerRepo.Split("/");
            return elements.Length == 2 ? (elements[0], elements[1]) : (string.Empty, string.Empty);
        }
    }
}
