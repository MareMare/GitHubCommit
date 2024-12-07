// --------------------------------------------------------------------------------------------------------------------
// <copyright file="FileEncoder.cs" company="MareMare">
// Copyright © 2024 MareMare.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
// </copyright>
// --------------------------------------------------------------------------------------------------------------------

namespace GitHubCommit.Services;

/// <summary>
/// ファイルをBASE64エンコードされた文字列に変換するためのユーティリティクラス。
/// </summary>
internal static class FileEncoder
{
    /// <summary>
    /// 指定されたファイルパスの内容を読み込み、BASE64エンコードされた文字列に変換します。
    /// </summary>
    /// <param name="sourceFilePath">エンコードするファイルのローカルパス。</param>
    /// <returns>BASE64エンコードされたファイル内容の文字列。</returns>
    /// <exception cref="FileNotFoundException">ファイルが見つからない場合にスローされます。</exception>
    /// <exception cref="IOException">ファイルの読み込み中にエラーが発生した場合にスローされます。</exception>
    public static string EncodeFileToBase64(string sourceFilePath)
    {
        if (!File.Exists(sourceFilePath))
        {
            throw new FileNotFoundException("The specified file was not found.", sourceFilePath);
        }

        try
        {
            var fileBytes = File.ReadAllBytes(sourceFilePath);
            return Convert.ToBase64String(fileBytes);
        }
        catch (IOException ex)
        {
            throw new IOException("An error occurred while reading the file.", ex);
        }
    }
}
