# Asp.Net Core 2.0
## 1.更新日志
[更新日志](md/log.md)
## 2. Swagger
[Swagger使用说明](md/Swagger.md)

## 3. API多版本支持
[多版本支持](md/ApiVersion.md)

## 更新功能
### 1.知识点
#### 1.1 增加Url网址配置文件
 项目默认使用http://localhost:5000的Url进行侦听，我们可以增加一个配置文件来随时修改Url地址。
 在项目根目录中增加一个hosting.json文件，文件内容如下(192.168.57.7是服务器IP)：
 ```
{
  "server.urls": "http://*:5000;https://*:5001"
}
```
这里的*是代表任何IP都可以访问，默认是localhost
编辑Program.cs文件，修改为内容如下：
```
    public class Program
    {
        public static void Main(string[] args)
        {
            var config = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("hosting.json",optional:true).Build();

            CreateWebHostBuilder(args)
                .UseConfiguration(config)
                .Build().Run();
        }

        public static IWebHostBuilder CreateWebHostBuilder(string[] args) =>
            WebHost.CreateDefaultBuilder(args)
                .UseStartup<Startup>();
    }
```

