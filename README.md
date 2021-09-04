# dotnet-tuto

## .NET の歴史

- **.NET Framework**は、2002年に最初に登場し、Windowsプラットフォームのみをサポート対象としてバージョンアップを続けてきた
    - 開発対象:
        - Windows上でのGUIアプリケーション（WPF, Windows Forms）
        - IISに配置するWebアプリケーション／サービス（ASP.NET）
        - Officeや Visual Studio の拡張
- 一方、.NET Framework をクロスプラットフォーム化するためのプロジェクトとして **Mono** プロジェクトがあった
    - Monoプロジェクトは .NET Framework の互換環境（GUIフレームワークを含む）を Linux や macOS を含めたスマートフォンOSにすることを目標とする
    - Monoプロジェクトは現在、Xamarin社が開発・サポートをしている
    - Xamarin社は、2016年にMicrosoft社に買収されて今に至っている
- Monoプロジェクトという先人がいる中、Microsoftは2014年11月12日に、.NET のクロスプラットフォーム実装である **.NET Core** を発表
    - 今までWindowsのみをサポート対象としてきた .NET 実装に Linux や macOS もサポート対象として加えられた
    - .NET Core は最初から GitHub でオープンソースとして公開された（MITライセンス）
        - オープンソース化は ASP.NET チームを中心にすでに進んでいる最中であり、Compiler as a Serviceとしての "Roslyn" プロジェクトもオープンソースとして開発が進んでいた最中の出来事でもあった
- .NET Core は、当初はサーバーサイドでの利用を想定して開発が進められていった
    - Mono が .NET Framework そのものと互換性のある環境（＝WPFなどOS依存機能を除くほぼフルセットの機能提供）を目指していることに対し、.NET Core は .NET Framework のサブセットとなる機能をクロスプラットフォームで提供することを目標とていた
    - 2015年11月5日にRed Hat社がMicrosoft社と協力し、Red Hat Enterprise Linux上での .NET のサポートを発表した
        - これにより、商用Linux環境でも .NET Core が公式サポートされることになった

### .NET Core
- 当初サーバサイド利用のみを想定されていた .NET Core だが、現在では以下の開発フレームワークを提供している
    - クラウド／Webアプリケーション
    - 機械学習／人工知能
    - IoT
    - GUIアプリケーション（Windows限定）
- .Net Core では以下の開発言語をサポート
    - C#
    - VB.NET
    - F#
- 実装について
    - .NET は共通言語基盤 (CLI) の元となっている仕様であり、.NET Core はCLIの実装になっている
    - .NET Core のランタイムは CoreCLR
        - CoreCLR はガベージコレクタ、JITコンパイラ（RyuJIT）、プリミティブな型/クラスライブラリから成る
        - ランタイムに必要な型/クラスライブラリに追加して、フレームワークとして規定する型群として CoreFX が .NET Core には含まれる
    - .NET Core と .NET Framework は API を一部共有しているが、.NET Core には .NET Framework に存在しない固有の API が搭載されていることに加え、CoreRT も含まれている
        - CoreRT とは、AOTコンパイルされたネイティブバイナリとの統合のために最適化された、.NET ネイティブランタイム
    - .NET Core ライブラリの派生物は UWP 用に利用されている
    - .NET Core のコマンドラインインタフェースは、オペレーティングシステムには実行エントリポイントを、開発者にはコンパイルやパッケージ管理などのサービスを提供する
- 2021年7月現在の最新評価版: 6.0.0-preview.1

***

## Environment

- OS:
    - Windows10
    - Ubuntu 20.04
- Shell:
    - PowerShell (Windows10)
    - Bash (Ubuntu 20.04)
- Editor:
    - VSCode
- .NET Core SDK: `2.1`
    - 外部DLL（.Net Framework 等で作成されたDLL）を直接参照可能なのは .NET Core 2.x のみ
        - ※ ただし、Windows プラットフォームのみサポート（Mac／Linux は非対応）
        - ※ .NET Core 3.x 以降は NuGet 経由でのDLL参照のみをサポート
    - 2021年9月時点で .NET Core は `3.1` がLTS版で `6.0` が最新版だが、外部DLLを直接参照するために `2.1` を利用する

### Setup

#### Windows10
`Win + X` |> `A` => 管理者権限 PowerShell 起動

```powershell
# install Chocolatey package manager
> Set-ExecutionPolicy Bypass -Scope Process -Force; [System.Net.ServicePointManager]::SecurityProtocol = [System.Net.ServicePointManager]::SecurityProtocol -bor 3072; iex ((New-Object System.Net.WebClient).DownloadString('https://chocolatey.org/install.ps1'))

# install .NET Core SDK 2.1
> choco install -y dotnetcore-sdk --version=2.1.802

# -- close and re-open powershell

# confirm dotnet version
> dotnet --version
2.1.802
```

#### Ubuntu 20.04
```bash
# register Microsoft package key
$ wget https://packages.microsoft.com/config/ubuntu/20.04/packages-microsoft-prod.deb -O packages-microsoft-prod.deb
$ sudo dpkg -i packages-microsoft-prod.deb

# install .NET Core SDK 2.1
$ sudo apt update && \
  sudo apt install -y apt-transport-https && \
  sudo apt update && \
  sudo apt install -y dotnet-sdk-2.1

# confirm dotnet version
$ dotnet --version
2.1.816
```

***

## Hello World

### プロジェクトの作成
```powershell
# プロジェクト作成
## - Type: Console
## - Name: HelloWorld
## - Directory: HelloWorld
> dotnet new console -n HelloWorld -o HelloWorld

# プロジェクトディレクトリに移動
> cd HelloWorld
```

### プロジェクトの構成
```bash
HelloWorld/
|_ bin/ # .exe, .dll 等のコンパイル済みバイナリファイルが格納される
|
|_ obj/ # 各種プロジェクト情報のデータファイルが格納される
|  |_ HelloWorld.csproject.nuget.dgspec.json
|  |_ HelloWorld.csproject.nuget.props
|  |_ HelloWorld.csproject.nuget.targets
|  |_ project.assets.json
|
|_ HelloWorld.csproj # コンパイル設定ファイル
|_ Program.cs # プロジェクトのエントリーファイル（メインプログラム）
```

### メインプログラム
プロジェクト作成時点で `Hello World!` とコンソール出力するプログラムコードが書かれているため確認

```csharp:Program.cs
// Program.cs

// System 名前空間のモジュールを読み込む
using System;

// HelloWorld 名前空間のモジュールを定義する
namespace HelloWorld
{
    // Program クラスを定義する
    class Program
    {
        // Main メソッドがプログラムのエントリーポイントになる
        // * Main メソッドをもつクラスが複数定義された場合にはコンパイルエラーが発生する
        static void Main(string[] args)
        {
            // コンソールに `Hello World!` と出力
            Console.WriteLine("Hello World!");
        }
    }
}
```

### プログラムのコンパイル・実行
プロジェクトディレクトリ（`Program.cs` ファイルのあるディレクトリ）で以下のコマンドを実行する

```powershell
# プログラムのコンパイル・実行
> dotnet run

Hello World!
```

***

## CeVIO AI Speak Test

.NET Core SDK で CeVIO AI の小春六花ちゃんを喋らせてみる

※ 前提として CeVIO AI 本体と小春六花ちゃんのトークボイスがインストールされている環境であること

### プロジェクトの作成
```powershell
# プロジェクト作成
## - Type: Console
## - Name: CevioTest
## - Directory: CevioTest
## - Target Framework: .Net 4.0
##   - CeVIO AI 連携APIのDLLが .Net Framework 4.8 で作られているため、Target Framework に 4.0 を指定する
> dotnet new console -n CevioTest -o CevioTest --target-framework-override net4.0

# プロジェクトディレクトリに移動
> cd CevioTest
```

### 外部DLLの準備
CeVIO AI のAPIを使うためには `CeVIO.Talk.RemoteService2.dll` をプロジェクトに追加する必要がある

```powershell
# CeVIO.Talk.RemoteService2.dll (CeVIO AI 連携API) を CeVIO AI 本体ディレクトリからコピー
> cp cp "C:\Program Files\CeVIO\CeVIO AI\CeVIO.Talk.RemoteService2.dll" .\

# プロジェクト設定ファイルを VSCode で開く
> code CevioTest.csproj
```

`CevioTest.csproj` ファイルに依存DLL `CeVIO.Talk.RemoteService2.dll` の設定を書き加える

```diff:CevioTest.csproj
  <Project Sdk="Microsoft.NET.Sdk">
  
    <PropertyGroup>
      <OutputType>Exe</OutputType>
      <TargetFramework>net4.0</TargetFramework>
    </PropertyGroup>

+   <ItemGroup>
+     <Reference Include="Cevio.Talk.RemoteService2.dll">
+       <HintPath>Cevio.Talk.RemoteService2.dll</HintPath>
+     </Reference>
+   </ItemGroup>

  </Project>
```

### CeVIO AI に喋らせるコードの記述
公式サイトの [.NET連携API仕様](https://cevio.jp/guide/cevio_ai/interface/dotnet/) を参考に `Program.cs` を記述する

```csharp:Program.cs
using System;
using CeVIO.Talk.RemoteService2;

namespace CevioTest
{
    class Program
    {
        static void Main(string[] args)
        {
            // 【CeVIO AI】起動
            ServiceControl2.StartHost(false);

            // Talkerインスタンス生成
            Talker2 talker = new Talker2();

            // キャスト設定
            talker.Cast = "小春六花";

            // （例）音量設定
            talker.Volume = 100;

            // （例）再生
            SpeakingState2 state = talker.Speak("こんにちは");
            state.Wait();

            // 【CeVIO AI】終了
            ServiceControl2.CloseHost();
        }
    }
}
```

### プログラムのコンパイル・実行
```powershell
# プログラムのコンパイル・実行
> dotnet run

## => CeVIO AI が起動し、立花ちゃんが「こんにちは」と喋ればOK
```
