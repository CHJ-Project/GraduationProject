using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace common
{
    public enum OperationCode
    {
        //服务器端需要响应的操作码
        login,                  //处理登录操作
        register,               //处理注册操作
        maching,                //处理匹配请求
        sethero,                //将玩家选择的角色类型发送给服务器端
        game,                   //处理服务器端接收客户端发来游戏中的位置，旋转，动画信息
        exit,                   //处理玩家退出游戏请求

        //客户端需要响应的操作码
        error,                  //处理错误信息
        loginsuccess,           //处理登录成功请求
        registersuccess,        //处理注册成功请求
        setselfcode,            //设置自身客户端的识别码
        setothercode,           //设置匹配到的玩家的识别码
        setgamecode,            //设置匹配成功的那场游戏的识别码
        setenemytype,           //设置对手选择的角色类型
        setscene,               //设置由服务器随机选择的场景作为本场游戏的场景
        setplayerinitialpos,    //设置由服务器随机生成的位置作为玩家角色的初始位置
        setenemyinitialpos      //设置有服务器随机生成的位置作为对手角色的初始位置
    }
}
