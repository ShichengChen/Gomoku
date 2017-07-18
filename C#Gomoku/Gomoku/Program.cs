using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Gomoku
{
    class Program
    {
        static final int start_y = 20;
	static final int start_x = 30;
	static final int chess_gap = 39;
	static final int chess_line_number = 15;
	static final int chess_length = chess_gap * (chess_line_number - 1);
	static final int chess_piece_radius = chess_gap / 2;
	static final int text_x = 700;
	static final int text_y = 150;
	static final int startButton_x = 700;
	static final int startButton_y = 250;
	public int step;
	public int fromX,fromY,toX,toY;
	static boolean sustain;
	boolean validStep;  
	AnchorPane anchorpane;
	Button startButton;
	Text notify;
	int chessboard[][];
	Computer computer;
        void firstPlay(){
		if(ifemptyDo(7,7))
			draw_cirle(start_x + 7 * chess_gap,start_y + 7 * chess_gap);
	}
        public void inite()
	{
		step = 0;
		fromX = fromY = chess_line_number - 1;
		toX = toY = 0;
		sustain = true;
		validStep = false;
		computer = new Computer();
		chessboard = new int[chess_line_number][chess_line_number];
		initButton();
		notify = new Text();
		anchorpane.getChildren().addAll(notify,startButton);
		for(int i = 0;i < chess_line_number;i++)
		{
			Line line1 = new Line(start_x + i * chess_gap,start_y,start_x + i * chess_gap,start_y + chess_length);
			Line line2 = new Line(start_x,start_y + i * chess_gap,start_x + chess_length,start_y + i * chess_gap);
			anchorpane.getChildren().addAll(line1,line2);
		}
		for(int i = 0;i < chess_line_number;i++)
			for(int j = 0;j < chess_line_number;j++)
				chessboard[i][j] = 0;
		anchorpane.setOnMouseClicked(new click_to_play_chess());
		startButton.setOnAction(new push_startButton());
		firstPlay();
	}
        boolean ifemptyDo(int xth,int yth){
		if(chessboard[yth][xth] != 0)return false;
		chessboard[yth][xth] = (step & 1) + 1;
		fromX = Math.min(fromX,xth);fromY = Math.min(fromY,yth);
		toX = Math.max(toX,xth);toY = Math.max(toY,yth);
		computer.set(fromX, toX, fromY, toY, chessboard);
		if(Tricks.fiveConsecutive(chessboard,xth,yth))
		{
			sustain = false;
			if(chessboard[yth][xth] == 1)
				showText(true);
			else 
				showText(false);
		}
		return true;
	}
        public class click_to_play_chess implements EventHandler<MouseEvent> {
		public void handle(MouseEvent actEvt) {
			double x = actEvt.getSceneX();
			double y = actEvt.getSceneY();
			if(x <= (start_x - chess_piece_radius) ||
			   y <= (start_y - chess_piece_radius))return;
			if(x >= chess_length + start_x + chess_piece_radius ||
			   y >= chess_length + start_y + chess_piece_radius)return;
			int xth = (int)((x - (start_x - chess_piece_radius)) / chess_gap);
			int yth = (int)((y - (start_y - chess_piece_radius)) / chess_gap);
			if(sustain)if(ifemptyDo(xth,yth))
			{
				draw_cirle(start_x + xth * chess_gap,start_y + yth * chess_gap);
				validStep = true;
			}
			
			////////computer run
			if(sustain && validStep)
			{
				computer.alphabeta(1, -1000000000, 1000000000, 0);
				xth = computer.cx;
				yth = computer.cy;
				if(ifemptyDo(xth,yth))
					draw_cirle(start_x + xth * chess_gap,start_y + yth * chess_gap);
				validStep = false;
			}
		}
	}
        static void Main(string[] args)
        {

        }
    }
}
