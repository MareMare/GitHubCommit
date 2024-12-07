// --------------------------------------------------------------------------------------------------------------------
// <copyright file="IGitHubClientService.cs" company="MareMare">
// Copyright © 2024 MareMare.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GitHubCommit.Services;

/// <summary>
/// GitHubと対話し、ログインやリポジトリ内のファイルの作成・更新を行うインターフェイスを表します。
/// </summary>
public interface IGitHubClientService
{
    /// <summary>
    /// パーソナルアクセストークンを使用してGitHubにログインします。
    /// </summary>
    /// <param name="token">GitHubのパーソナルアクセストークン。nullの場合は環境変数 "GH_TOKEN" を使用します。</param>
    /// <returns>非同期のログイン操作を表すタスク。</returns>
    /// <exception cref="Exception">ログインに失敗した場合にスローされます。</exception>
    ValueTask LoginAsync(string? token = null);

    /// <summary>
    /// 指定されたGitHubリポジトリにファイルを作成または更新します。
    /// </summary>
    /// <param name="owner">リポジトリの所有者。</param>
    /// <param name="repo">リポジトリの名前。</param>
    /// <param name="branch">ファイルを作成または更新するブランチ。</param>
    /// <param name="targetFilePath">作成または更新するファイルのパス。</param>
    /// <param name="contentBase64String">ファイルのBase64エンコードされた内容。</param>
    /// <param name="message">ファイルの作成または更新のコミットメッセージ。</param>
    /// <returns>非同期操作を表すタスク。</returns>
    /// <exception cref="InvalidOperationException">GitHubクライアントが初期化されていない場合にスローされます。</exception>
    ValueTask CreateOrUpdateFileAsync(
        string owner,
        string repo,
        string branch,
        string targetFilePath,
        string contentBase64String,
        string? message);
}
