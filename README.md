# Easemob.Restfull4Net
环信Restfull API dotnet的封装  
支持的.Net Framework版本：4.0   
环信Restfull API地址：[http://docs.easemob.com/start/100serverintegration](http://docs.easemob.com/start/100serverintegration)  

#### 一、SDK的使用说明:   
采用配置节点的形式来设置环信，且一次可以支持多个app进行调用，在数据处理上，全部采用强类型实体进行包装，异常处理全部数据错误日志，方便查询。  
#####以下为配置节点的使用方法：  
1、采用Section的形式配置app，这种方式有个好处，可以连续配置多个app。  
```
<configSections>
  <section name="EasemobServer" type="Easemob.Restfull4Net.Config.Configuration.ServerConfigSection,Easemob.Restfull4Net"/>
</configSections>
<EasemobServer>
  <!--
  备注：此节点可添加多个
  serverUrl：环信服务器地址
  orgName：组织名，对应#前面部分
  appName：应用名，对应#后面部分
  clientId：客户端ID
  clientSecret：客户端密钥
  httpTimeOut：请求超时设置（以毫秒为单位）
  isDebug：是否为调试模式，说明：如果为调试模式，将在程序主目录输出日志文件
  maxJsonLength：JavaScriptSerializer类接受的JSON字符串的最大长度
  -->
  <server 
    serverUrl="https://a1.easemob.com/" 
    orgName="orgName" 
    appName="appName1" 
    clientId="clientId1" 
    clientSecret="clientSecret1" 
    httpTimeOut="10000" 
    isDebug="true" 
    maxJsonLength="0">
  </server>
  <server
    serverUrl="https://a1.easemob.com/"
    orgName="orgName"
    appName="appName2"
    clientId="clientId2"
    clientSecret="clientSecret2"
    httpTimeOut="10000"
    isDebug="true"
    maxJsonLength="0">
  </server>
</EasemobServer>
```
2、使用自定义节点配置，目前SDK封装的只能使用一个app。
```
<add key="HX_EaseServerUrl" value="https://a1.easemob.com/"/><!--环信服务器地址-->
<add key="HX_EaseAppClientID" value="clientId3"/><!--客户端ID-->
<add key="HX_EaseAppClientSecret" value="clientSecret3"/><!--客户端密钥-->
<add key="HX_EaseAppName" value="appName3"/><!--应用名，对应#后面部分-->
<add key="HX_EaseAppOrgName" value="orgName"/><!--组织名，对应#前面部分-->
<add key="HX_EaseHttpTimeOut" value="10000"/><!--请求超时设置（以毫秒为单位）-->
<add key="HX_EaseIsDebug" value="true"/><!--是否为调试模式，说明：如果为调试模式，将在程序主目录输出日志文件-->
<add key="HX_EaseMaxJsonLength" value="0"/><!--JavaScriptSerializer类接受的JSON字符串的最大长度-->
```
3、使用代码的硬编码形式启动app。
```
//自定义实例化
var syncRequest = new SyncRequest(new ServerConfig()
{
    OrgName = "",
    AppName = "",
    ClientId = "",
    ClientSecret = "",
    IsDebug = true,
});
```
#####以下为实际代码使用   
在SDK内容，已经将配置的节点都封装进Client，在使用的时候只要直接使用Client提供的方法即可   
如：我想在创建一个用户时的代码如下:   
```
//单个创建
var user = Client.DefaultSyncRequest.UserCreate(new UserCreateReqeust()
{
    nickname = string.Concat("Test", this._userName, "3"),
    password = "123456",
    username = string.Concat("Test", this._userName, "3"),
});
Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
```
如：我集成了多个app时，DefaultSyncRequest默认返回第一个，而我要使用特定的某个app时，代码如下：
```
//单个创建
var user = Client.SyncRequests["app2"].UserCreate(new UserCreateReqeust()
{
    nickname = string.Concat("Test", this._userName, "3"),
    password = "123456",
    username = string.Concat("Test", this._userName, "3"),
});
Assert.AreEqual(user.StatusCode, HttpStatusCode.OK);
```
其中SyncRequests中的key就是你在环信的appname。
####二、目前SDK的封装进度：   
#####同步请求：  
【完成】用户体系集成  
【完成】聊天记录  
【完成】文件上传下载  
【完成】发送消息  
【未完成】群组管理  
【未完成】聊天室管理  
#####异步请求：
【未开始】  
######（备注：以上为目前的开发进度，且上面完成的功能都已经投入使用，后续将完成剩下的api封装；目前整个SDK都是使用同步请求进行，还未加入异步请求，后续也将集成进去。）  
####三、版本更新历史  
*******************************************************  
version：1.0.1  data：2016-08-22  
1.修改单元测试项目，其中测试的图片上传采用网络图片  
2.加入NAnt进行持续集成，如果要采用NAnt进行build，需要确定本机安装啦NAnt工具，且由于项目中的单元测试项目使用的是MSTest，所以要确保编译的机器上已经安装此组件；NAnt.build文件中有设置依赖组件的组件，打开编辑成所在编译机器路径即可  
*******************************************************  
  
*******************************************************  
version：1.0.0  data：2016-08-10  
初始版本发布  
*******************************************************  
####四、项目相关地址  
源码：[https://github.com/easonjim/Easemob.Restfull4Net](https://github.com/easonjim/Easemob.Restfull4Net)  
bug提交：[https://github.com/easonjim/Easemob.Restfull4Net/issues](https://github.com/easonjim/Easemob.Restfull4Net/issues)  
Release版本：[https://github.com/easonjim/Easemob.Restfull4Net/releases](https://github.com/easonjim/Easemob.Restfull4Net/releases)  
NuGet：[https://www.nuget.org/packages/Easemob.Restfull4Net/](https://www.nuget.org/packages/Easemob.Restfull4Net/)  
####License
Easemob.Restfull4Net is licensed under the MIT License.
