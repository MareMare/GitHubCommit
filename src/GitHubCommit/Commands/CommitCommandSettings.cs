// --------------------------------------------------------------------------------------------------------------------
// <copyright file="CommitCommandSettings.cs" company="MareMare">
// Copyright © 2024 MareMare.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using JetBrains.Annotations;
using Spectre.Console.Cli;

namespace GitHubCommit.Commands;

/// <summary>
/// GitHubと対話し、ログインやリポジトリ内のファイルの作成・更新を行うコマンド設定を表します。
/// </summary>
[UsedImplicitly]
public sealed class CommitCommandSettings : CommandSettings
{
    /// <summary>
    /// コミットするローカルファイルのパスを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>コミットするローカルファイルのパス。</para>
    /// </value>
    [CommandArgument(0, "<SourceFilePath>")]
    [Description("Specify the local path of the file to commit.")]
    [Required]
    public string SourceFilePath { get; set; } = default!;

    /// <summary>
    /// リポジトリ内で作成または更新するファイルのパスを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>リポジトリ内で作成または更新するファイルのパス。</para>
    /// </value>
    [CommandArgument(1, "<TargetFilePath>")]
    [Description("Specify the path of the file to create or update in the repository.")]
    [Required]
    public string TargetFilePath { get; set; } = default!;

    /// <summary>
    /// OWNER/REPOフォーマットを使用してリポジトリ名を取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>OWNER/REPOフォーマットを使用してリポジトリ名。</para>
    /// </value>
    [CommandOption("--repo <OwnerRepo>")]
    [Description("Select another repository using the OWNER/REPO format.")]
    [DefaultValue("OWNER/REPO")]
    public string OwnerRepo { get; set; } = default!;

    /// <summary>
    /// ファイルを作成または更新するブランチを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>ファイルを作成または更新するブランチ。</para>
    /// </value>
    [CommandOption("--branch <Branch>")]
    [Description("Specify the branch where the file will be created or updated.")]
    [DefaultValue("main")]
    public string Branch { get; set; } = "main";

    /// <summary>
    /// ファイル作成または更新のコミットメッセージを取得または設定します。
    /// </summary>
    /// <value>
    /// 値を表す <see cref="string" /> 型。
    /// <para>ファイル作成または更新のコミットメッセージ。</para>
    /// </value>
    [CommandOption("--message <Message>")]
    [Description("Specify the commit message for the file creation or update.")]
    [Required]
    public string Message { get; set; } = default!;
}
