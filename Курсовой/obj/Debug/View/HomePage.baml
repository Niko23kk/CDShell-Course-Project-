   M S B A M L   `   `   `  ����K  GКурсовой, Version=1.0.0.0, Culture=neutral, PublicKeyToken=nullG(clr-namespace:Курсовой.ViewModelКурсовой.ViewModel  5      =#clr-namespace:Курсовой.ViewКурсовой.View  5      M ISystem, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089M3clr-namespace:System.ComponentModel;assembly=SystemSystem.ComponentModel 5      $    Курсовой.View.HomePage   5      R NWindowsBase, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35\ XPresentationFramework, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35W SPresentationCore, Version=4.0.0.0, Culture=neutral, PublicKeyToken=31bf3856ad364e35D 9http://schemas.microsoft.com/winfx/2006/xaml/presentation    5      R NSystem.Xaml, Version=4.0.0.0, Culture=neutral, PublicKeyToken=b77a5c561934e089:x,http://schemas.microsoft.com/winfx/2006/xaml     5      Jmc;http://schemas.openxmlformats.org/markup-compatibility/2006     5      @d2http://schemas.microsoft.com/expression/blend/2008     5      7vm(clr-namespace:Курсовой.ViewModel     5      5local#clr-namespace:Курсовой.View     5      Dmode3clr-namespace:System.ComponentModel;assembly=System     5      -   5        $��HomePageName��5      $	��600��5
        N� Title$  HomePage��5       � DataContext 5      2  ,Курсовой.ViewModel.HomePageViewModel  5   
   .��5      �    BackgroundImage#���   6    � RowDefinitions 5   
   �� 5      $��40 �6   �� 5      -   5         �Name$ 
MainHeight��5      $��* �6.   �� 5      $��40 �6    � ColumnDefinitions 5   
   �� 5      $
��1.5* �6   �� 5      $��* �6   .+�5   
   ]� $��1��6   $��0��6!   $��
20,10,5,10q�61   $	��300��6E   .+�5      ��  
 TextBl#���  6   $	��Top=�69     _LastPtojects#���  6N   $��32(�6u   $
��Wrap=�6�    �� Padding$ 10q�6�   $��30,10q�6�   �� 5      -   5        $��ScrollContainer��5      ��.  $��Hidden=�6X   $
��auto��6}   .��5       �� ��6   �� *ProjectsCurentUser+ �� Mode$ TwoWay=� �� UpdateSourceTrigger$ PropertyChanged=���5!      �� 5"      .R�5#      �  5$   "   �� 5%   &   $��* �67   �� 5&   &   $��50 �67   �� 5'   &   $��50 �67   .+�5)   "   �� $��SelectedProject��6)   $��2q�6@     Gradient#���  6K    �� CornerRadius$ 10��6s   $��1q�6�   
�������	 J� InputBindings	 5*   &   d� 5+   *   
 d� MouseAction$
 	LeftClicke�67    �� Command 6O   �� *DataContext.OpenProject+ �� RelativeSource �� *FindAncestor+ �� AncestorType" �� �� CommandParameter 6�   ��  �� ElementName$ SelectedProject��._�5-   &   � $��15q�6+   $��Center=�67    5.   *   �� 5/   .   $��2* �6?   �� 50   .   $��* �6?   �� 51   .   $��40 �6?   .+�53   *   �� $��0��64   $
��Wrap=�6b   $��17(�6v   ��6D   �� *Project_name+6v   �� 54   *   $��1��64   $
��Wrap=�6�   $��17(�6�   ��6D   �� *Date_of_change+ �� StringFormat$ dd-MM-yy hh:mm��6�   �� 55   *   $��2��63   $
��Left=�6C   $
��10,0q�6{   $��20��6�   $��20��6�   $��20q�6�    d� 	IsChecked 6^   �� *Status+6�   56   '   57   #   �� 59   "   $��1��6)   $��Center=�69   $��Center=�6V   $ 50��6q   ���	#B2E8E8E8 �� Padding$ 10q�6�   ._�5:   &   �� $9��3pack://application:,,,/Resources/Images/Preview.png��6-   $��25��6j    � ToolTip 5;   &   �� 5<   *   $	��250��6K   $	��250��6W   ��60   �� *	Preview+6W   5>   #   �� 5@   "   $��2��6)   $��Center=�69   $��Center=�6V   $ 50��6q     _Delete# �  6�   ���	#B2E8E8E8$ 10q�6�   ._�5A   &   �� $8��2pack://application:,,,/Resources/Images/Delete.png��6-   $��25��6i   	 5B   &   d� 5C   *   $
 	LeftClicke�67    6P   �� *DataContext.DeleteProject+ �� *FindAncestor+" �� 6�   �� $ SelectedProject��5E   #   5G      5H      5O      5P      5Q      �� 5S   
   $��1��6   $��1��6$   ��.  $��Hidden=�6Z   $
��auto��6   .��5T      �� $��10,10,20,30q�6   .+�5U      ]� .+�5V      �� $ 20��6   $ 10 10 20 20q�6/     UserGradient#���  6E    J� Effect 5W      3  -System.Windows.Media.Effects.DropShadowEffect  5X         
BlurRadius$ 10Q�6/   ._�5Z      � .+�5[      A� $��80��6&   $��80��62   $
��20,0q�6=   ��5\   "   �� 5]   &    �� ImageSource 61   �� *Avatar+5_      �� 5`      $��Right=�6)   $��Center=�6E   .+�5a   "   �� #���  6,   $��24(�6d   $
��10,5q�6r   ��6M   �� *Login+6r   � 5b   "   .+�5c   &   �� #���  60   $��18(�6l   $
��10,3q�6z   ��6Q   �� *	FirstName+6z   �� 5d   &   #���  60   $��18(�6m   $
��10,3q�6{   ��6Q   �� *
SecondName+6{   5e   #   �� 5f   "   #���  6,   $��18(�6d   $
��10,3q�6r   ��6M   �� *Email+6r   5g      5h      5i      5j      �� 5l        NoStyleButtonWithoutWhite#��[ 6   ��6L   �� *NewProjectClick+��6p   �� $ HomePageName��.��5m      �� $��10,20q�6   $ 20��6,   $
��auto��6>   $��Center=�6K     ButtonGradient#���  6h    5n        5o      $ 10Q�6/   ._�5r      � $
��0,20q�6$   $��Center=�62   .+�5s      �� $
��20,0q�6$   $��Center=�62   $��30��6O   $��30��6[   $5��/pack://application:,,,/Resources/Images/add.png��6f   �� 5t      #���  6(   $
��Wrap=�6I     _NewProject#���  6]   $��Center=�6�   $��24(�6�   $	��150��6�   $��	10,0,30,0q�6�   5u      5v      5w      �� 5y      #��[ 6   ��6L   �� *AddTextureClick+.��5z      �� $��10,20q�6   $ 20��6,   $
��auto��6>   $��Center=�6K   #���  6h    5{        5|      $ 10Q�6/   ._�5~      � $
��0,20q�6$   $��Center=�62   .+�5      �� $
��20,0q�6$   $��Center=�62   $��30��6O   $��30��6[   $5��/pack://application:,,,/Resources/Images/add.png��6f   �� 5�      #���  6(   $
��Wrap=�6I    	 
_NewObject#��� 	 6]   $��Center=�6�   $��24(�6�   $	��150��6�   $��	10,0,30,0q�6�   5�      5�      5�      �� 5�      #��[ 6   ��6L   �� *LoadProject+.��5�      �� $��10,20q�6   $ 20��6,   $
��auto��6>   $��Center=�6K   #���  6h    5�        5�      $ 10Q�6+   ._�5�      � $
��0,20q�6    $��Center=�6.   .+�5�      �� $
��20,0q�6    $��Center=�6.   $��30��6K   $��30��6W   $5��/pack://application:,,,/Resources/Images/add.png��6b   �� 5�      #���  6(   $
��Wrap=�6I    
 _LoadProject#��� 
 6]   $��Center=�6�   $��24(�6�   $	��150��6�   $��	10,0,30,0q�6�   5�      5�      5�      5�      5�      5�      5�      