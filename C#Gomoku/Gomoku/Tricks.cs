using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    public class Tricks
    {
        public static int[][] d = { new int[] { 1, 0 }, new int[] { -1, 0 }, new int[] { 0, 1 }, new int[] { 0, -1 }, new int[] { 1, 1 }, new int[] { -1, -1 }, new int[] { 1, -1 }, new int[] { -1, 1 } };
	    public static  int[][] position = { 
	        new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 }, 
	        new int[]{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, 
	        new int[]{ 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 },
	        new int[]{ 0, 1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 4, 4, 4, 4, 4, 4, 4, 3, 2, 1, 0 },
	        new int[]{ 0, 1, 2, 3, 4, 5, 5, 5, 5, 5, 4, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 4, 5, 6, 6, 6, 5, 4, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 4, 5, 6, 7, 6, 5, 4, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 4, 5, 6, 6, 6, 5, 4, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 4, 5, 5, 5, 5, 5, 4, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 4, 4, 4, 4, 4, 4, 4, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 3, 3, 3, 3, 3, 3, 3, 3, 3, 2, 1, 0 }, 
	        new int[]{ 0, 1, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 2, 1, 0 }, 
	        new int[]{ 0, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 1, 0 }, 
	        new int[]{ 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0, 0 } };
	    public static  int c_line_number = 15;
	    public static int curx,cury;
	    ///which just have be blocked
	    public static bool valid(int xth,int yth){
		    return xth >= 0 && yth >= 0 && yth < c_line_number && xth < c_line_number;
	    }
	    public static bool equ(int xth,int yth,int x,int y,int [][] c){
		    return (valid(x,y) && c[yth][xth] == c[y][x]);
	    }
	    public static bool equZero(int x,int y,int [][] c){
		    return (valid(x,y) && c[y][x] == 0);
	    }
	    public static bool equ(int x,int y,int [][]c,int n){
		    return valid(x,y) && c[y][x] == n;
	    }
	    public static bool increase(int xth,int yth,int ix,int iy,int[] ans,int [][]c){
		    int x = xth + ix,y = yth + iy;
		    for(;equ(xth,yth,x,y,c);x += ix,y += iy)
			    ans[0]++;
		    return valid(x,y) && c[y][x] == 0;
	    }
	    public static bool fiveConsecutive(int [][] c,int xth,int yth)
	    {
		    int []ans = new int[1];
		    for(int i = 0;i < 4;i++)
		    {
			    ans[0] = 1;
			    increase(xth,yth,d[2 * i][0],d[2 * i][1],ans,c);
			    increase(xth,yth,d[2 * i + 1][0],d[2 * i + 1][1],ans,c);
			    if(ans[0] >= 5)return true;
		    }
		    return false;
	    }
	    public static bool AliveFour(int [][] c,int xth,int yth)
	    {
		    int []ans = new int[1];
		    bool have;
		    for(int i = 0;i < 4;i++)
		    {
			    ans[0] = 1;
			    have = increase(xth,yth,d[2 * i][0],d[2 * i][1],ans,c);
			    have = (have && increase(xth,yth,d[2 * i + 1][0],d[2 * i + 1][1],ans,c));
			    if(ans[0] == 4 && have)return true;
		    }
		    return false;
	    }
	    public static int deathsFour(int [][] c,int xth,int yth)
	    {
		    if(AliveFour(c,xth,yth))return 0;
		    int cnt = 0;
		    for(int i = 0;i < 8;i++)
		    {
			    int x = xth + d[i][0],y = yth + d[i][1];
			    for(;equ(xth,yth,x,y,c);x += d[i][0],y += d[i][1]);
			    //until to first empty chessboard
			    if(equZero(x,y,c))
			    {
				    c[y][x] = c[yth][xth];
				    if(fiveConsecutive(c,x,y))
					    cnt++;
				    c[y][x] = 0;
			    }
		    }
		    return cnt;
	    }
	    public static int AliveThree(int [][] c,int xth,int yth)
	    {
		    int cnt1 = 0,cnt2 = 0;
		    for(int i = 0;i < 8;i++)
		    {
			    int x = xth,y = yth;
			    for(;equ(xth,yth,x - d[i][0],y - d[i][1],c);
					    x -= d[i][0],y -= d[i][1]);
			    int a = c[yth][xth];
			    int _x2 = x - 2 * d[i][0],_y2 = y - 2 * d[i][1];
			    int _x1 = x - 1 * d[i][0],_y1 = y - 1 * d[i][1];
			    int x1 = x + 1 * d[i][0],y1 = y + 1 * d[i][1];
			    int x2 = x + 2 * d[i][0],y2 = y + 2 * d[i][1];
			    int x3 = x + 3 * d[i][0],y3 = y + 3 * d[i][1];
			    int x4 = x + 4 * d[i][0],y4 = y + 4 * d[i][1];
			    //because,above code is sub,so this code is add 
			    ///////0011100
			    if(equ(_x1,_y1,c,0) && equ(x1,y1,c,a) && equ(x2,y2,c,a) && equ(x3,y3,c,0))
			    {
				    if(equZero(_x2,_y2,c) && equZero(x4,y4,c))
					    cnt1++;
			    }
			    //////011010
			    else if(equ(_x1,_y1,c,0) && equ(x1,y1,c,a) && 
					    equ(x2,y2,c,0) && equ(x3,y3,c,a) && equ(x4,y4,c,0))
				    cnt2++;
			    //////010110
			    else if(equ(_x1,_y1,c,0) && equ(x1,y1,c,0) && 
					    equ(x2,y2,c,a) && equ(x3,y3,c,a) && equ(x4,y4,c,0))
				    cnt2++;
		    }
		    return cnt1 / 2 + cnt2;
	    }
	    public static int AliveTwo(int [][] c,int xth,int yth)
	    {
		    int ans = 0;
		    for(int i = 0;i < 8;i++)
		    {
			    int _x1 = xth - d[i][0],_y1 = yth - d[i][1];
			    int _x2 = xth - 2 * d[i][0],_y2 = yth - 2 * d[i][1];
			    int x1 = xth + d[i][0],y1 = yth + d[i][1];
			    int x2 = xth + 2 * d[i][0],y2 = yth + 2 * d[i][1];
			    int x3 = xth + 3 * d[i][0],y3 = yth + 3 * d[i][1];
			    int x4 = xth + 4 * d[i][0],y4 = yth + 4 * d[i][1];
			    ////////
			    //AliveTwo
			    ////////
			    ////////001100
			    if(equ(_x2,_y2,c,0) && equ(_x1,_y1,c,0) && equ(xth,yth,x1,y1,c) &&
			       equ(x2,y2,c,0) && equ(x3,y3,c,0))
				    ans++;
			    ////////0010100
			    else if(equ(_x2,_y2,c,0) && equ(_x1,_y1,c,0) && equ(x1,y1,c,0) &&
					    equ(xth,yth,x2,y2,c) && equ(x3,y3,c,0) && equ(x4,y4,c,0))
				    ans++;
			    ////////010010
			    else if(equ(_x1,_y1,c,0) && equ(x1,y1,c,0) && equ(x2,y2,c,0) &&
					    equ(xth,yth,x3,y3,c) && equ(x4,y4,c,0))
				    ans++;
		    }
		    return ans;
	    }
	    public static void blockDeathsFour(int [][] c,int xth,int yth)
	    {
		    for(int i = 0;i < 8;i++)
		    {
			    int x = xth + d[i][0],y = yth + d[i][1];
			    for(;equ(xth,yth,x,y,c);x += d[i][0],y += d[i][1]);
			    //until to first empty chessboard
			    if(equZero(x,y,c))
			    {
				    c[y][x] = c[yth][xth];
				    if(fiveConsecutive(c,x,y))
				    {
					    curx = x;
					    cury = y;
					    c[y][x] = 0;
					    return;
				    }
				    c[y][x] = 0;
			    }
		    }
	    }
    }
}
