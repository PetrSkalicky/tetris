   M S B A M L   `   `   `  ����A  =tetris, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullclr-namespace:tetristetris  5   	       tetris.MainWindow   5   	   \ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35e aSystem.Windows.Controls.Ribbon, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35F 9http://schemas.microsoft.com/winfx/2006/xaml/presentation     5   	   R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5   	   @d2http://schemas.microsoft.com/expression/blend/2008     5   	   Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5   	   &localclr-namespace:tetris     5   	   -   5          � Title$  Tetris��5   	   $	��745��6   $	��490��6%    � 
ResizeMode$ CanMinimize=�61    � WindowStartupLocation$ CenterScreen=�6z   ��6�   �� *
Background+��6�   Z� .��5	      � .+�5
   
   �� -   5         ��Name$ 
ScoreLabel��5
      $
��Left=�6J   $��332,443,0,0q�6e   $	��Top=�6z    J� RenderTransformOrigin$ -0.842,-0.346&�6�   $��92��6�   $	��117��6�   $��20(�6�   $��Consolas)�6�   ���	#FFC89632��6$   �� *ScoreLabelContent+6�   �� 5   
   -   5         ��Name$ 
MainCanvas��5      $
��Left=�6%   $	��662��6@   $��	20,27,0,0q�6M   $	��Top=�6`   $	��302��6x   �� 5   
   -   5         ��Name$ StartButton��5      $��Start new game��6&   $
��Left=�6?   $��
332,27,0,0q�6Z   $	��Top=�6n   $	��132��6�   $��30��6�   !  System.Windows.Media.Colors   Transparent#���  6�   ���	#FF3CBD3C$��16(�6  $��Consolas)�6  ���	#FFAFAFAF60  �� 5   
   -   5        $ PauseButton��5      $
��Left=�6&   $��
332,67,0,0q�6A   $	��Top=�6U   $	��132��6m   $��30��6�   #���  6�   ���	#FFC89632$��16(�6  $��Consolas)�6)  ���	#FFAFAFAF��6�   �� *PauseButtonContent+6?  �� 5   
   -   5        $ 
NextCanvas��5      $
��Left=�6%   $	��106��6@   $��334,211,0,0q�6M   $	��Top=�6b   $	��128��6z   �� 5   
   $��Next:��6   $
��Left=�6    $��329,173,0,0q�6;   $	��Top=�6P   $��33��6h   $	��112��6t   $��20(�6�   $��Consolas)�6�   ���	#FFC896326�   �� 5   
   -   5        $ HiScoreLabel��5      $
��Left=�6N   $��332,540,0,0q�6i   $	��Top=�6~   $ -0.842,-0.346&�6�   $��56��6�   $	��117��6�   $��20(�6�   $��Consolas)�6�   ���	#FFC89632��6&   �� *HiScoreLabelContent+6�   �� 5   
   -   5        $ AboutButton��5      $��About��6&   $
��Left=�66   $��332,659,0,0q�6Q   $	��Top=�6f   $	��132��6~   $��30��6�   #���  6�   ���	#FFC89632$��16(�6  $��Consolas)�6  ���	#FFAFAFAF6(  �� 5   
   ���	#FFAFAFAF$��1q�6)   $
��Left=�6=   $	��664��6X   $��	19,26,0,0q�6e   $	��Top=�6x   $	��304��6�   �� 5   
   -	   5        $ 
LevelLabel��5      $
��Left=�6J   $��332,382,0,0q�6e   $	��Top=�6z   $��Consolas)�6�   $��20(�6�   ���	#FFC89632��6$   �� *LevelLabelContent+6�   �� 5   
   -
   5        $ 	TimeLabel��5      $
��Left=�6H   $��332,322,0,0q�6c   $	��Top=�6x   $��Consolas)�6�   $��20(�6�   ���	#FFC89632��6#   �� *TimeLabelContent+6�   �� 5   
   -   5        $ NewLabel��5      $��NEW HISCORE!��6"   $
��Left=�6s   $��330,596,0,0q�6�   $	��Top=�6�   $	��142��6�   $��Consolas)�6�   $��20(�6�   ${�False��6  ��69   �� *Brush+y�6V   �� *Visib+��6�   Z� 6  5      5      