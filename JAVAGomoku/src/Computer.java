import java.util.ArrayList;
import java.util.Collections;



public class Computer {
	int x0,x1,y0,y1;
	int c[][];
	public int cx,cy;
	static final int VCF_depth = 10;
	static final int chess_line_number = 15;
	static final int d[][] = {{1,0},{-1,0},{0,1},{0,-1},{1,1},{-1,-1},{1,-1},{-1,1}};
	public void set(int _x0,int _x1,int _y0,int _y1,int [][]chess)
	{
		x0 = Math.max(Math.min(x0, _x0 - 2), 0);
		x1 = Math.min(Math.max(x1, _x1 + 2), chess_line_number - 1);
		y0 = Math.max(Math.min(y0, _y0 - 2), 0);
		y1 = Math.min(Math.max(y1, _y1 + 2), chess_line_number - 1);
		c = chess;
		cx = cy = 0;
	}
	boolean inTwoStep(int x,int y,int [][]c){
		for(int i = 0;i < 8;i++){
			int x1 = x + 1 * d[i][0],y1 = y + 1 * d[i][1];
			int x2 = x + 2 * d[i][0],y2 = y + 2 * d[i][1];
			if(Tricks.valid(x1, y1) && c[y1][x1] != 0)return true;
			if(Tricks.valid(x2, y2) && c[y2][x2] != 0)return true;
		}
		return false;
	}
	int getProbability(int [][]c,int x,int y,int player,int depth){
		c[y][x] = player;
		if(Tricks.fiveConsecutive(c, x, y))
			return Valuation.C5probability;
		c[y][x] = 3 - player;
		if(Tricks.fiveConsecutive(c, x, y))
			return Valuation._C5probability;
		
		c[y][x] = player;
		if(Tricks.AliveFour(c, x, y))return Valuation.A4probability;
		c[y][x] = 3 - player;
		if(Tricks.AliveFour(c, x, y))return Valuation._A4probability;
		
		c[y][x] = player;
		int a3 = Tricks.AliveThree(c, x, y);
		int d4 = Tricks.deathsFour(c, x, y);
		if(d4 >= 2 || (d4 > 1 && a3 > 1))return Valuation.DD4probability;
		c[y][x] = 3 - player;
		int _a3 = Tricks.AliveThree(c, x, y);
		int _d4 = Tricks.deathsFour(c, x, y);
		if(_d4 >= 2 || (_d4 > 1 && _a3 > 1))return Valuation._DD4probability;
		
		if(a3 >= 2)return Valuation.DA3probability;
		else if(_a3 >= 2)return Valuation._DA3probability;
		
		c[y][x] = player;
		int a2 = Tricks.AliveTwo(c, x, y);
		c[y][x] = 3 - player;
		int _a2 = Tricks.AliveTwo(c, x, y);
		c[y][x] = 0;
		int cur4 = d4 * Valuation.D4probability;
		int cur3 = a3 * Valuation.A3probability;
		int cur2 = a2 * Valuation.A2probability;
		int _cur4 = _d4 * Valuation.D4probability;
		int _cur3 = _a3 * Valuation.A3probability;
		int _cur2 = _a2 * Valuation.A2probability;
		return cur4 + cur3 + cur2 + _cur4 + _cur3 + _cur2;
	}
	public int alphabeta(int player,int alpha,int beta,int depth)
	{
		ArrayList<Score>s = new ArrayList<Score>();
		for(int x = 0;x < chess_line_number;x++)
		{
			for(int y = 0;y < chess_line_number;y++)
			{
				if(c[y][x] == 0)
				{
					if(!inTwoStep(x,y,c))continue;
					//if(inTwoStep(x,y,c))System.out.println();
					int cur = getProbability(c,x,y,player,depth);
					c[y][x] = 0;
					s.add(new Score(x,y,cur));
				}
			}
		}
		Collections.sort(s);
	    for(int i = 0;i < Math.min(15, s.size());i++)
	    {
//	    	if(player == 1 && s.get(0).score <= Valuation._A4probability && depth == 0)
//	        {
//        		int cur = VCF(0,depth);
//        		if(cur != 0)System.out.println("VCF!!!!!");
//    			if(cur == 1)return Valuation.INF[player];
//    			else if(cur == 2)return Valuation.INF[player];		
//	        }////在没有危险或者对方将形成33/34/44/4时尝试VCF绝杀
	    	if(depth == 0)
	    	{
	    		System.out.println("----------------------------------------");
	    		System.out.println("x: " + s.get(i).x + " y: " + s.get(i).y);
	    		System.out.println("----------------------------------------");
	    	}
    		if(s.get(0).score == Valuation.DA3probability || 
	           s.get(0).score == Valuation.DD4probability ||
	           s.get(0).score == Valuation.C5probability)
	        {
	        	set(s.get(i).x,s.get(i).y,depth);
	        	return Valuation.INF[player];
	        }
	        /////我方能够比对方先制胜！，返回。
	        if(s.get(i).score < Valuation._DA3probability && 
	           s.get(0).score >= Valuation._DA3probability)continue;
	        if(s.get(i).score < Valuation._A4probability && 
	 	       s.get(0).score >= Valuation._A4probability)continue;
	        if(s.get(i).score < Valuation._C5probability && 
	 	 	   s.get(0).score >= Valuation._C5probability)continue;
	        ////必须选择走在对方快赢的步位上
	        int v;
	        if(depth < 5)
	        {
	        	c[s.get(i).y][s.get(i).x] = player;
	        	v = alphabeta(3 - player,alpha,beta,depth + 1);
	        	c[s.get(i).y][s.get(i).x] = 0;
	        }
	        else{
	        	c[s.get(i).y][s.get(i).x] = player;
	        	v = Valuation.valBlack(x0, x1, y0, y1, c,player);
	        	c[s.get(i).y][s.get(i).x] = 0;
	        }
	        if(player == 1)
	        {
	        	if(v > alpha){
	        		alpha = v;
	        		set(s.get(i).x,s.get(i).y,depth);
	        	}
	        }
	        else{
	        	if(v < beta){
	        		beta = v;
	        		set(s.get(i).x,s.get(i).y,depth);
	        	}
	        }
	        if(beta <= alpha)break;
	    }
	    return (player == 1) ? alpha : beta;
	}
	void set(int x,int y,int depth)
	{
		if(depth == 0)
		{
			cx = x;
			cy = y;
		}
	}
//	int VCF(int depth,int alphabetaD)
//	{
//		if(depth > VCF_depth)return 0;
//		for(int x = x0;x <= x1;x++)
//		{
//			for(int y = y0;y <= y1;y++)
//			{
//				if(c[y][x] == 0)
//				{
//					c[y][x] = 1;
//					int cur = 0;
//					if(Tricks.fiveConsecutive(c, x, y))	
//						cur = 2;
//					else if(Tricks.AliveFour(c, x, y))
//						cur = 1;
//					else if(Tricks.deathsFour(c, x, y) > 1)
//						cur = 1;
//					else if(Tricks.deathsFour(c, x, y) > 0 && Tricks.AliveThree(c, x, y) > 0)
//						cur = 1;
//					if(Tricks.deathsFour(c, x, y) > 0)
//					{
//						Tricks.blockDeathsFour(c, x, y);
//						cur = VCF(depth + 1,alphabetaD);
//						c[Tricks.cury][Tricks.curx] = 0;
//					}
//					c[y][x] = 0;
//					if(cur > 0)
//					{
//						if(depth == 0 && alphabetaD == 0)set(x,y,alphabetaD);
//						return cur;
//					}
//				}
//			}
//		}
//		return 0;
//	}
	public class Score implements Comparable<Score>
	{
		public int x,y,score;
		public Score(int _x,int _y,int _score){
			x = _x;
			y = _y;
			score = _score;
		}
		public int compareTo(Score s){
			if(score < s.score)return 1;
			else if(score == s.score)return 0;
			else return -1;
		}
	}
}
