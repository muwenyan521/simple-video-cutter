# GitHub Actions Workflows

本项目包含以下GitHub Actions工作流配置：

## 工作流概览

### 1. CI (ci.yml)
**触发条件**: 推送到main/develop分支，或创建PR到main/develop分支

**功能**:
- 多平台构建 (x64, x86)
- 多配置构建 (Debug, Release)
- 代码测试
- 代码覆盖率报告
- 安全扫描
- 代码风格检查

### 2. Build and Release (build.yml)
**触发条件**: 推送到main/develop分支，创建PR到main分支，或发布新版本

**功能**:
- 构建应用程序
- 创建发布包
- 自动发布到GitHub Releases

### 3. Release (release.yml)
**触发条件**: 发布新版本时

**功能**:
- 构建x64和x86版本
- 包含FFmpeg
- 创建发布包
- 自动发布到GitHub Releases

### 4. Dependencies (dependencies.yml)
**触发条件**: 每周一自动运行，或手动触发

**功能**:
- 检查过时的依赖包
- 检查有安全漏洞的包
- 自动更新依赖包（手动触发时）

## 使用方法

### 自动构建
1. 推送代码到main或develop分支
2. CI工作流会自动运行
3. 检查构建状态和测试结果

### 发布新版本
1. 在GitHub上创建新的Release
2. 设置版本标签（如v1.2.0）
3. 发布工作流会自动运行
4. 生成x64和x86版本的发布包

### 手动更新依赖
1. 在GitHub仓库页面
2. 进入Actions标签页
3. 选择"Dependencies"工作流
4. 点击"Run workflow"
5. 选择"update-dependencies"分支
6. 点击"Run workflow"

## 环境变量

工作流使用以下环境变量：
- `DOTNET_VERSION`: .NET版本 (7.0.x)
- `FFMPEG_VERSION`: FFmpeg版本 (6.1)

## 输出文件

### 构建产物
- `SimpleVideoCutter-x64-{version}.zip`: x64版本发布包
- `SimpleVideoCutter-x86-{version}.zip`: x86版本发布包

### 包含文件
- SimpleVideoCutter.exe
- ffmpeg.exe
- ffprobe.exe
- README.md
- LICENSE
- 所有必要的.NET运行时文件

## 故障排除

### 构建失败
1. 检查.NET版本是否正确
2. 确认所有依赖包都已正确安装
3. 查看构建日志中的具体错误信息

### 发布失败
1. 确认GitHub Token权限正确
2. 检查Release标签格式是否正确
3. 确认FFmpeg安装成功

### 依赖更新失败
1. 检查网络连接
2. 确认包管理器配置正确
3. 查看更新日志中的错误信息

## 自定义配置

### 修改构建配置
编辑相应的工作流文件中的以下部分：
- 构建平台: 修改`platform`矩阵
- 构建配置: 修改`configuration`矩阵
- .NET版本: 修改`DOTNET_VERSION`环境变量

### 添加新的工作流
1. 在`.github/workflows/`目录下创建新的.yml文件
2. 定义触发条件和执行步骤
3. 提交并推送更改

## 注意事项

1. 确保所有工作流文件使用正确的YAML语法
2. 定期检查依赖包的安全更新
3. 在发布前充分测试构建产物
4. 保持工作流配置与项目需求同步 