using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public class Valuation
    {
	    public static  int [][] d = {new int[]{1,0},new int[]{0,1},new int[]{1,1},new int[]{1,-1}};
	    public static  int c_line_number = 15;
	    public static  int []deathsThreeScore = {0,10,-10};
	    public static  int []AliveTwoScore = {0,10,-10};
	    public static  int []AliveThreeScore = {0,200,-200};
	    public static  int []deathsFourScore = {0,200,-200};//1e2,1e2
	    public static  int []doubleAliveThreeScore = {0,10000,-10000};//1e4,1e4
	    public static  int []doubleDeathsFourScore = {0,1000000,-1000000};//1e6,1e6
	    public static  int AliveThreeAndDeathsFourScore = {0,1000000,-1000000};//1e6,1e6
	    public static  int []AliveFourScore = {0,1000000,-1000000}; //1e6,1e6
	    public static  int []fiveConsecutiveScore = {0,100000000,-100000000}; //1e8
	    public static  int []INF = {0,100000001,-100000001}; //1e8
	    public static  int valuable = 1000000;
	
    /////last score	
    /////////////////////////////////////////////////////////////////////////////////////	
    /////probability to search this node	 
	    public static  int C5probability = 300000000;
	    public static  int _C5probability = 100000000;//1e8
	
	    public static  int A3D4probability = 30000000;//3 * 1e7
	    public static  int A4probability = 30000000;
	    public static  int DD4probability = 30000000;
	    public static  int _A3D4probability = 10000000;//1e7
	    public static  int _A4probability = 10000000;
	    public static  int _DD4probability = 10000000;
	
	    public static  int DA3probability = 3000000;//3 * 1e6
	    public static  int _DA3probability = 1000000;//1e6
	
	    public static  int A3probability = 200;
	    public static  int D4probability = 200;
	    public static  int D3probability = 10;
	    public static  int A2probability = 10;
	
	    public static bool valid(int xth,int yth){
		    return xth >= 0 && yth >= 0 && yth < c_line_number && xth < c_line_number;
	    }
	    public static bool equal(int xth,int yth,int x,int y,int [][] c){
		    return (valid(x,y) && c[yth][xth] == c[y][x]);
	    }
	    public static bool equalZero(int x,int y,int [][] c){
		    return (valid(x,y) && c[y][x] == 0);
	    }
	    public static bool nEqual(int xth,int yth,int x,int y,int [][] c){
		    return !equal(xth,yth,x,y,c);
	    }
	    public static bool equal(int x,int y,int [][]c,int a){
		    return valid(x,y) && c[y][x] == a;
	    }
	    public static bool block(int x,int y,int [][]c,int a){
		    return !valid(x,y) || c[y][x] == a;
	    }
	    public static bool BlockN1(int xth,int yth,int x,int y,int [][] c,int a){
		    if(equal(xth,yth,c,0) && equal(x,y,c,3 - a))return true;
		    if(equal(xth,yth,c,3 - a) && equal(x,y,c,0))return true;
		    if(equalZero(xth,yth,c) && !valid(x,y))return true;
		    if(equalZero(x,y,c) && !valid(xth,yth))return true;
		    return false;
	    }
	    public static int valBlack(int x0,int x1,int y0,int y1,int [][]c,int player)
	    {
		    int ans = 0;
		    //int ans1 = 0,ans2 = 0,ans3 = 0,ans4 = 0,ans5 = 0;
		    bool []level1 = {false,false,false};//34,44,4
		    bool []level2 = {false,false,false};//33
		    bool []level3 = {false,false,false};//d4;
		    bool []level4 = {false,false,false};//3;
		    for(int x = x0;x <= x1;x++)
		    {
			    for(int y = y0;y <= y1;y++)
			    {
				    if(c[y][x] != 0)
				    {
					    ans += AliveThree(x,y,c) * AliveThreeScore[player];
					    ans += deathsFour(x,y,c) * deathsFourScore[player];
					    ans += deathsThreeOrAliveTwo(x,y,c) * AliveTwoScore[player];
					    if(c[y][x] == 1)
					    {
						    if(Tricks.AliveFour(c, x, y))
							    level1[1] = true;
						    int a3 = Tricks.AliveThree(c, x, y);
						    int d4 = Tricks.deathsFour(c, x, y);
						    if(a3 > 0)level4[1] = true;
						    if(d4 > 0)level3[1] = true;
						    if(d4 >= 2 || (d4 > 1 && a3 > 1))
							    level1[1] = true;
						    else if(a3 >= 2)
							    level2[1] = true;
					    }
					    else
					    {
						    if(Tricks.AliveFour(c, x, y))
							    level1[2] = true;
						    int _a3 = Tricks.AliveThree(c, x, y);
						    int _d4 = Tricks.deathsFour(c, x, y);
						    if(_a3 > 0)level4[2] = true;
						    if(_d4 > 0)level3[2] = true;
						    if(_d4 >= 2 || (_d4 > 1 && _a3 > 1))
							    level1[2] = true;
						    else if(_a3 >= 2)
							    level2[2] = true;
					    }
				    }
			    }
		    }
		    //1//34,44,4
		    //2//33
		    //3//d4;
		    //4//3;
		    if(level1[3 - player])return fiveConsecutiveScore[3 - player];
		    if(level2[3 - player])return doubleAliveThreeScore[3 - player];
		    if(level3[3 - player])return fiveConsecutiveScore[3 - player];
		    if(level1[player])return AliveFourScore[player];
		    if(level2[player])return doubleAliveThreeScore[player];
		    return ans;
    //		if(level4[3 - player] && !level3[player])
    //			return doubleAliveThreeScore[3 - player];
    //		if(level1[3 - player] || level2[3 - player] || level3[3 - player])
    //			return INF[3 - player];
    //		else if(level4[3 - player] && !level3[player])
    //			return INF[3 - player];
    //		else if(level1[player] || level2[player])
    //			return INF[player];
    //		System.out.println("deathsThreeOrAliveTwo: " + ans1);
    //		System.out.println("AliveThree: " + ans2);
    //		System.out.println("deathsFour: " + ans3);
    //		System.out.println("AliveFour: " + ans4);		
    //		System.out.println("fiveConsecutive: " + ans5);
	    }
	    public static int deathsThreeOrAliveTwo(int xth,int yth,int [][]c)
	    {
		    int ans = 0;
		    for(int i = 0;i < 4;i++)
		    {
			    int _x1 = xth - d[i][0],_y1 = yth - d[i][1];
			    int _x2 = xth - 2 * d[i][0],_y2 = yth - 2 * d[i][1];
			    int x1 = xth + d[i][0],y1 = yth + d[i][1];
			    int x2 = xth + 2 * d[i][0],y2 = yth + 2 * d[i][1];
			    int x3 = xth + 3 * d[i][0],y3 = yth + 3 * d[i][1];
			    int x4 = xth + 4 * d[i][0],y4 = yth + 4 * d[i][1];
			    int a = c[yth][xth];
			    ////////
			    //deathsThree
			    ///////
			    if(equal(xth,yth,x1,y1,c) && equal(xth,yth,x2,y2,c))
			    {	
				    ////211100
				    if((equal(x3,y3,c,0) && equal(x4,y4,c,0) && block(_x1,_y1,c,3 - a)) || 
				       (equal(_x2,_y2,c,0) && equal(_x1,_y1,c,0) && block(x3,y3,c,3 - a)))
					    ans++;
				    ////2011102
				    else if(equal(x3,y3,c,0) && equal(_x1,_y1,c,0) && 
						    block(x4,y4,c,3 - a) && block(_x2,_y2,c,3 - a))
					    ans++;
				    continue;
			    }
			    ///////210110
			    else if(equal(x1,y1,c,0) && equal(xth,yth,x2,y2,c) && equal(xth,yth,x3,y3,c))
			    {
				    if(BlockN1(_x1,_y1,x4,y4,c,a))
					    ans++;
				    continue;
			    }
			    //////211010
			    else if(equal(xth,yth,x1,y1,c) && equal(x2,y2,c,0) && equal(xth,yth,x3,y3,c))
			    {
				    if(BlockN1(_x1,_y1,x4,y4,c,a))
					    ans++;
				    continue;
			    }
			    //////10101
			    else if(equal(xth,yth,x4,y4,c))
			    {
				    if(c[y1][x1] == a && c[y2][x2] == 0 && c[y3][x3] == 0)
					    ans++;
				    if(c[y1][x1] == 0 && c[y2][x2] == a && c[y3][x3] == 0)
					    ans++;
				    if(c[y1][x1] == 0 && c[y2][x2] == 0 && c[y3][x3] == a)
					    ans++;
				    continue;
			    }
			    ////////
			    //AliveTwo
			    ////////
			    ////////001100
			    if(equal(_x2,_y2,c,0) && equal(_x1,_y1,c,0) && equal(xth,yth,x1,y1,c) &&
			       equal(x2,y2,c,0) && equal(x3,y3,c,0))
				    ans++;
			    ////////0010100
			    else if(equal(_x2,_y2,c,0) && equal(_x1,_y1,c,0) && equal(x1,y1,c,0) &&
					    equal(xth,yth,x2,y2,c) && equal(x3,y3,c,0) && equal(x4,y4,c,0))
				    ans++;
			    ////////010010
			    else if(equal(_x1,_y1,c,0) && equal(x1,y1,c,0) && equal(x2,y2,c,0) &&
					    equal(xth,yth,x3,y3,c) && equal(x4,y4,c,0))
				    ans++;
		    }
		    return ans;
	    }
	
	    public static int AliveThree(int xth,int yth,int [][]c)
	    {
		    int ans = 0;
		    for(int i = 0;i < 4;i++)
		    {
			    int _x1 = xth - d[i][0],_y1 = yth - d[i][1];
			    int _x2 = xth - 2 * d[i][0],_y2 = yth - 2 * d[i][1];
			    int x1 = xth + d[i][0],y1 = yth + d[i][1];
			    int x2 = xth + 2 * d[i][0],y2 = yth + 2 * d[i][1];
			    int x3 = xth + 3 * d[i][0],y3 = yth + 3 * d[i][1];
			    int x4 = xth + 4 * d[i][0],y4 = yth + 4 * d[i][1];
			    int a = c[yth][xth];
			    ///////0011100
			    if(equal(_x1,_y1,c,0) && equal(x1,y1,c,a) && equal(x2,y2,c,a) && equal(x3,y3,c,0))
			    {
				    if(equalZero(_x2,_y2,c) && equalZero(x4,y4,c))
					    ans++;
			    }
			    //////011010
			    else if(equal(_x1,_y1,c,0) && equal(x1,y1,c,a) && 
					    equal(x2,y2,c,0) && equal(x3,y3,c,a) && equal(x4,y4,c,0))
				    ans++;
			    //////010110
			    else if(equal(_x1,_y1,c,0) && equal(x1,y1,c,0) && 
					    equal(x2,y2,c,a) && equal(x3,y3,c,a) && equal(x4,y4,c,0))
				    ans++;
		    }
		    return ans;
	    }
	
	    public static int deathsFour(int xth,int yth,int [][]c)
	    {
		    int ans = 0;
		    for(int i = 0;i < 4;i++)
		    {
			    int _x1 = xth - d[i][0],_y1 = yth - d[i][1];
			    int x1 = xth + d[i][0],y1 = yth + d[i][1];
			    int x2 = xth + 2 * d[i][0],y2 = yth + 2 * d[i][1];
			    int x3 = xth + 3 * d[i][0],y3 = yth + 3 * d[i][1];
			    int x4 = xth + 4 * d[i][0],y4 = yth + 4 * d[i][1];
			    int a = c[yth][xth];
			    ///////011112
			    ///////211110
			    if(equal(x1,y1,c,a) && equal(x2,y2,c,a) && equal(x3,y3,c,a))
			    {
				    if((equal(_x1,_y1,c,0) && block(x4,y4,c,3 - a)) || 
				       (equal(x4,y4,c,0) && block(_x1,_y1,c,3 - a)))
					    ans++;
			    }
			    //////11101
			    //////11011
			    //////10111
			    else if(equal(xth,yth,x4,y4,c))
			    {
				    if(c[y1][x1] == a && c[y2][x2] == a && c[y3][x3] == 0)
					    ans++;
				    if(c[y1][x1] == 0 && c[y2][x2] == a && c[y3][x3] == a)
					    ans++;
				    if(c[y1][x1] == a && c[y2][x2] == 0 && c[y3][x3] == a)
					    ans++;
			    }
		    }
		    return ans;
	    }
	    public static int AliveFour(int xth,int yth,int [][]c)
	    {
		    int ans = 0;
		    //////011110
		    for(int i = 0;i < 4;i++)
		    if(equalZero(xth - d[i][0],yth - d[i][1],c))
		    {
			    int x = xth + d[i][0],y = yth + d[i][1];
			    int cnt = 1;
			    for(;equal(xth,yth,x,y,c);x += d[i][0],y += d[i][1])
				    cnt++;
			    if(cnt == 4)ans++;
		    }
		    return ans;
	    }
	    public static int fiveConsecutive(int xth,int yth,int [][]c)
	    {
		    /////11111
		    for(int i = 0;i < 4;i++)
		    {
			    int x = xth + d[i][0],y = yth + d[i][1];
			    int cnt = 1;
			    for(;equal(xth,yth,x,y,c);x += d[i][0],y += d[i][1])
				    cnt++;
			    if(cnt >= 5)return 1;
		    }
		    return 0;
	    }
    }

}
