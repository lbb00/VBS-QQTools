/*
 * @author onelong
 * @github 
 * @time 2016.11.14
 * @version v0.1.0
 * @compliant 安卓QQv6.5.5 
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
    // startAction = 1
    startAction = Int(ReadUIConfig("action", 0))
    
    // 获取点赞数量
    // times = 20
    times = int(ReadUIConfig("times",20))
    
    // 启动相对应的功能
    // 0 赞附近的人
    // 1 回赞
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
	
    // 基线初始化为-1，寻找基线用到的3个点
    Dim blueY = -1,baseY1,baseY2,baseY3
    baseY1 = 229
    baseY2 = 223
    baseY3 = 230
    If CmpColorEx("0|" & baseY1 & "|E0A500-50000,0|" & baseY2 & "|FFFFFF-10000,0|" & baseY3 & "|FFFFFF-10000", 1) = 1 Then blueY = baseY1
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
    	
        // 寻找本屏第一条分片分隔线
        FindMultiColor 0, y1 + 1, 320, screenY, "E0DFDE-50000", "160|0|E0DFDE-50000,315|0|E0DFDE-50000", 0, 1, x1, y1
    	
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
            FindMultiColor 0, y1 + 1, 320, screenY, "E0DFDE-50000", "160|0|E0DFDE-50000,315|0|E0DFDE-50000", 0, 1, x2, y2
    	
            // 如果两条线之间距离为172，则存在一个名片
            If y2 - y1 = 172 Then 

                // 判断是否是广告，不是广告则进一步操作
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
                    TracePrint "广告"
                	
                End If
                
              
                //  将第二条分隔线y2复制给y1
                y1 = y2
            End If
            
            // 如果找不到下一条线，则进行操作结束
            If y2 = -1 Then 
                TracePrint "exti carryon"
                Exit While
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
    While isIntoNearbyCard = -1
        // 如果没有加载成功，则等待1s后再次判断
        Delay 1000
    Wend
    
    // 开始点赞
    startThumbUpNearby
End Function

/*
 * @func 判断是否是广告\主播
 * @param [startY] {int} 名片顶部基线y坐标
 * @return {int} 1或-1,1为是广告，-1为不是广告
 */
Function isAd(startY)
    Dim x ,y
	
    // 寻找是否有主播标志
    FindColor 585, startY + 43, 696, startY + 67, "9DD210-101010", 0, 0.9, x, y
    // 如果有主播标志返回1，结束函数
    If x > -1 and y > -1 Then 
        isAd = 1
        Exit Function
    End If
	
    // 寻找是否有广告标志
    FindColor 196, startY + 74, 249, startY + 106, "7F7EFF-101010", 0, 0.9, x, y
    // 如果有广告标志返回1，结束函数
    If x > -1 and y > -1 Then 
        isAd = 1
        Exit Function
    End If
	
    // 如果不是广告也不是主播返回-1
    isAd = -1
End Function

/*
 * @func 是否进入附近的人名片
 * @return {int} 1进入，-1没有进入
 */
Function isIntoNearbyCard()
    If CmpColorEx("490|1200|F2B91E-101010",0.9) = 1 Then
        isIntoNearbyCard = 1
    Else
        isIntoNearbyCard = -1
    End If
End Function

/*
 * @func 开始点赞
 */
Function startThumbUpNearby()

    // 休息 0.5秒
    Delay 500
	
    Dim x,y
    x = screenX - 40
    y = 700
    
    // 循环点赞10次
    For 10
    
        // 休息0.3秒
        Delay 300
        
        // 由于部分手机状态栏透明导致qq整体会上移50,点赞图标范围下方50pi以内仍然有效，应对位置做出适当调整
        // 区域高度距点赞边框30，上移后点击有效范围最底部比原点赞边框底部高出20，点赞边框高度为54，因此点赞位置距标准点赞边框底部上调35
        // 点击赞的位置
        onclick(x,y)
    Next
    
    // 休息0.5s
    Delay 500
    
End Function

/*
 * @func 封装点击
 * @param [x] {int} x坐标
 * @param [y] {int} y坐标
 */
Function onclick(x, y)

    // 按下
    TouchDown x, y, 0
	
    // 休息0.3s
    Delay 300
	
    // 抬起
    TouchUp 0
	
End Function


/*
 * @func 回赞
 * @param  
 */
Function backToPraise
		
    // 基线初始化为-1，寻找基线用到的3个点
    Dim blueY = -1,baseY1,baseY2,baseY3
    baseY1 = 229
    baseY2 = 223
    baseY3 = 230
    If CmpColorEx("0|" & baseY1 & "|E0A500-50000,0|" & baseY2 & "|FFFFFF-10000,0|" & baseY3 & "|FBF9F9-10000", 1) = 1 Then blueY = baseY1
    
    // 基线下方有个灰条，所以基线为：
    Dim baseY = blueY + 76
    
    TracePrint baseY
    
    // 寻找基线
    Dim x1,y1,x2,y2
    
    // 是否进行下一屏操作
    Dim isNextScreen = 1
    
    // 记录当前的操作数
    Dim count = 0
    
    // 开始进行第1-第N屏操作
    While isNextScreen = 1
    	
        // 每屏开始时设置y1为baseY
        y1 = baseY
    	
        // 本屏第一条名片分隔线为baseY
        y1 = baseY
    	
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
            FindMultiColor 0, y1 + 1, 320, screenY, "E0DFDE-50000", "160|0|E0DFDE-50000,315|0|E0DFDE-50000", 0, 1, x2, y2
    		
            // 如果没找到下一条分隔线，本屏循环结束
            If y2 = -1 Then 
            	
                // 本屏循环结束
                Exit While
            
                // 如果下一条分隔线与上一条分隔线间距大于或等于172，则存在一个名片
                // 名片上下分隔线之间为172，但有些名片上面有个日期的分栏，多出距离为76左右
            ElseIf y2 - y1 >= 172 Then
            	
                // 开始进行对该名片回赞
                startBackToPraise (y2)
                
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
        
        // 休息1s
        Delay 1000
    	
        TracePrint "y1"&y1
        // 翻屏滚动
        Swipe 200, y1 - 50, 200, baseY + 76 - 10, 3000
    	
        // 休息1.5s
        Delay 1500
    	
    Wend

End Function

/*
 * @func 点击回赞
 * @param [endY] {int} 名片底部y坐标
 * @note 
 */
Function startBackToPraise(endY)
	
    // 确定点赞的位置
    Dim x,y
    x = screenX - 40
    y = endY - 30
	
    // 有的人设置了陌生人不可点赞，点击时应对名片作一下判断
    // 判断名片上是否有回赞图标,如果没有跳过对本名片点赞
    If CmpColorEx(x & "|" & y & "|FFFFFF-101010", 0.9) = 1 Then 
	 
        // 跳过本名片点赞
        Exit Function
    End If
	
    // 休息0.5s
    Delay 500
	
    // 循环点击10次
    For 10
	
        onclick x, y
		
        // 休息0.3s
        Delay 300		
    Next
	
End Function