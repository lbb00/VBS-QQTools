
/*
 * @author onelong
 * @github 
 * @time 2017.8.9
 * @version v0.4.0
 * @compliant 安卓QQv7.1.5
 */

// 屏幕的宽高
Dim screenX,screenY

// 启动功能
Dim startAction

// 点赞数量
Dim times

// 主程序调用
main 

/*
 * @func 主程序
 */
Function main()
    init()
End Function

/*
 * @func 程序初始化
 */
Function init()

    // 获取屏幕宽高
    screenX = GetScreenX()
    screenY = GetScreenY()
    
    // 获取启动功能
	startAction = Int(ReadUIConfig("action"))
    
    // 获取点赞数量
    times = 20
    // times = int(ReadUIConfig("times"))
    
    // 启动相对应的功能
    // 0 赞附近的人
    // 1 回赞
    // 2 厘米秀
    Select Case startAction
    Case 0
        thumbUpNearby 
    Case 1
        backToPraise 
    End Select
    
End Function

/*
 * @func 赞附近的人
 */
Function thumbUpNearby()

    Dim blueY	
    If CmpColorEx("0|216|FFFFFF,21|217|CECECE,22|217|E0A500",0.9) = 1
        TracePrint "位置正确"
        blueY = 172
    End If
    
    // 寻找基线
    Dim x1,y1,x2,y2
    
    // 是否进行下一屏操作
    Dim isNextScreen = 1
    
    // 记录当前的操作数
    Dim count = 0
    
    // 开始进行第1-第N屏操作
    While isNextScreen = 1
    	
        // 每屏开始时设置y1为blueY
        y1 = blueY
    	
        // 寻找本屏第一条分片分隔线
        FindMultiColor 0, y1 + 1, 200, screenY,"E0DFDE","93|0|E0DFDE,144|0|E0DFDE",0,1, x1, y1
    	
        // 初始化y2为y1+1
        y2 = y1 +1
    	
        TracePrint "next"
        // 开始进行找下一个名片的操作
    	
        While True
        	
            TracePrint "carryon"
        
            // 如果操作数量达到了
            If count = times Then 
        		
                //退出
                Exit Function
        		
            End If
        	
            // 寻找下一条分隔线
            FindMultiColor 0, y1+1 , 300, screenY,"E0DFDE","93|0|E0DFDE,144|0|E0DFDE", 0, 1, x2, y2
    		
            TracePrint y1
            TracePrint y2
            // 如果两条线之间距离为188,或大于188（附近人自己名片下的第一个名片），则存在一个名片或主播，广告为180
            If y2 - y1 >= 188 Then 

                // 判断是否是主播，不是主播则进一步操作
                If isAd(y1) = -1 Then 
                 	
                    // 进入名片并点赞
                    intoNearbyCard(y2)
                	
                    // 计数器加1
                    count = count + 1
                    
                    // 回退到名片列表页
                    KeyPress "Back"
                    
                    // 休息1s
                    Delay 1000
                Else 
                    TracePrint "主播"
                End If
            End If
            
            

            // 如果找不到下一条线，则进行操作结束
            If y2 = -1 Then 
                TracePrint "exti carryon"
                Exit While
            Else 
                //  将第二条分隔线y2复制给y1
                y1 = y2
            End If
            
            
        Wend
        
        // 休息1s
        Delay 1000
    	
        // 翻屏滚动
        Swipe 200, y1 - 50, 200, blueY - 10, 2500
    	
        // 休息3s
        Delay 3000
    	
    Wend
    
    // 休息1秒
    Delay 1000
End Function

/*
 * @func 点击附近的人名片
 * @param [endY] {int} 名片底部基线y坐标
 */
Function intoNearbyCard(endY)

    // 点击名片底线上方50的地方
    onclick(100, endY - 50)
	
    // 点击后休息2s
    Delay 2000
	
    // 循环判断是否成功加载附近的人名片
    While isIntoNearbyCard() = -1
        // 如果没有加载成功，则等待1s后再次判断
        Delay 1000
    Wend
    
    // 开始点赞
    startThumbUpNearby
End Function

/*
 * @func 判断是否是主播
 * @param [startY] {int} 名片顶部基线y坐标
 * @return {int} 1或-1,1为是主播，-1为不是主播
 */
Function isAd(startY)
    Dim x ,y
	
    // 寻找是否有主播标志
    FindColor ScreenX-120, startY + 90, ScreenX-25, startY + 120, "9AD008-101010", 0, 0.9, x, y
    // 如果有主播标志返回1，结束函数
    If x > -1 and y > -1 Then 
        isAd = 1
        Exit Function
    End If
    
    isAd = -1
End Function

/*
 * @func 是否进入附近的人名片
 * @return {int} 1进入，-1没有进入
 */
Function isIntoNearbyCard
    Dim intX,intY
    FindMultiColor 0,screenY-200,50,screenY,"CCC8C3","0|-1|FFFFFF,0|1|F9F9F9",0,0.9,intX,intY
    If intX > -1 And intY > -1 Then
        isIntoNearbyCard = 1
    Else 
        isIntoNearbyCard = -1
    End If
End Function

/*
 * @func 开始点赞
 */
Function startThumbUpNearby()

    // 休息 0.2秒
    Delay 200
	
    Dim x,y
    
    //寻找点赞图标区域为590,658,575,543
    //判断是否存在点赞图标
    FindMultiColor screenX-130,598,screenX-30,757,"FFFFFF","21|-15|FFFFFF,42|13|FFFFFF,22|29|FFFFFF,-2|23|CDCDCA",0,0.9,x,y
    If x > -1 And y > -1 Then 
        TracePrint "找到点赞位置"
    Else 
        TracePrint x
        TracePrint y
        Exit Function
    End If
	
    // 休息0.15s
    Delay 150
	
    // 循环点击10次
    For 10
	
        onclick x, y
		
        // 休息0.1s
        Delay 100	
    Next
    
End Function
 
/*
 * @func 封装点击
 * @param [x] {int} x坐标
 * @param [y] {int} y坐标
 */
Function onclick(x, y)

    // 按下
    TouchDown x, y, 0
	
    // 休息0.1s
    Delay 100
	
    // 抬起
    TouchUp 0
	
End Function


/*
 * @func 回赞
 * @param  
 */
Function backToPraise
    Dim blueY
    Dim fx,fy
    FindMultiColor 160,160,180,screenY,"CECECE","1|0|E0A500,0|-1|FFFFFF,1|-1|E0A500",0,1,fx,fy
    If fx > -1 And fy > -1 Then 
        TracePrint "确定基线"
        blueY = fy
    Else 
        Exit Function
    End If

    
    TracePrint blueY
    
    // 寻找基线
    Dim x1,y1,x2,y2
    
    // 是否进行下一屏操作
    Dim isNextScreen = 1
    
    // 记录当前的操作数
    Dim count = 0
    
    // 开始进行第1-第N屏操作
    While isNextScreen = 1
    	
        // 每屏开始时设置y1为blueY
        y1 = blueY

    	
        TracePrint "next"
        // 开始进行找下一个名片的操作
    	
        While True
        	
            TracePrint "carryon"
        
            // 如果操作数量达到了
            If count = times Then 
        		
                // 退出
                Exit Function
        		
            End If
        	
            // 寻找下一条分隔线
            FindMultiColor 0, y1 + 1, 150, screenY, "E0DFDE", "65|0|E0DFDE,143|0|E0DFDE", 0, 1, x2, y2
            TracePrint y2
    		
            // 如果没找到下一条分隔线，本屏循环结束
            If y2 = -1 Then 
            	
                // 本屏循环结束
                Exit While
            
                // 如果下一条分隔线与上一条分隔线间距等于144，则存在一个名片
            ElseIf y2 - y1 = 172 Then
            	TracePrint "ok"
            	
                // 开始进行对该名片回赞
                startBackToPraise(y2)
                
                // 将第二条分隔线y2复制给y1
                y1 = y2
                
                // 计数器加1
                // @bug 这个地方，无论对方设置是否可以回赞计数器都加了1
                count = count + 1
            
                // 如果量条分隔线小于172，则是baseY和第一个名片间的间距，跳过这个位置   
            ElseIf y2 - y1 < 172

                // 将第二条分隔线y2复制给y1
                y1 = y2
                
            End If
           
        Wend
        
        // 休息0.5s
        Delay 500
    	
        TracePrint "y1"&y1
        // 翻屏滚动
        Swipe 200, y1 - 50, 200, blueY + 76 - 10, 3000
    	
        // 休息1s
        Delay 1000
    	
    Wend

End Function

/*
 * @func 点击回赞
 */
Function startBackToPraise(endY)

    // 判断该名片是否已经被点过 
    Dim intX,intY
    FindColor screenX-100,endY-50,screenX,endY,"F5B712",0,0.9,intX,intY
    If intX > -1 And intY > -1 Then 
        Exit Function
    End If
	
    Dim x=screenX-50,y=endY-45
    
    
	
    // 休息0.15s
    Delay 150
	
    // 循环点击10次
    For 10
	
        onclick x, y
		
        // 休息0.1s
        Delay 100	
    Next
	
End Function
