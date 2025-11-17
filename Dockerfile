# =======================================
# 1. ビルド用コンテナ（.NET SDK）
# =======================================
FROM mcr.microsoft.com/dotnet/sdk:8.0 AS build
WORKDIR /src

# プロジェクトファイルをコピー
COPY ./ActivityManagementApp.csproj ./
RUN dotnet restore

# 残りのソース全部コピー
COPY . ./

# Releaseビルド（publish）
RUN dotnet publish -c Release -o /app/out


# =======================================
# 2. 実行用コンテナ（.NET ランタイム）
# =======================================
FROM mcr.microsoft.com/dotnet/aspnet:8.0 AS final
WORKDIR /app

# ビルド成果物をコピー
COPY --from=build /app/out .

# Kestrel を 5000番ポートで待ち受け
EXPOSE 5000
ENV ASPNETCORE_URLS=http://0.0.0.0:5000

# アプリ起動
ENTRYPOINT ["dotnet", "ActivityManagementApp.dll"]
