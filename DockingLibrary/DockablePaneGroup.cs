using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;
using System.Xml;

namespace DockingLibrary
{
    /// <summary>
    /// How group panes are splitted
    /// </summary>
    public enum SplitOrientation
    {
        Horizontal,
        Vertical
    }

    /// <summary>
    /// Group of one or more child groups
    /// </summary>
    public class DockablePaneGroup : ILayoutSerializable
    {
        /// <summary>
        /// Pane directly attached
        /// </summary>
        public Pane AttachedPane { get; private set; }
        public Docks Dock { get; private set; }
        public DockablePaneGroup ParentGroup { get; internal set; }

        public DockablePaneGroup FirstChildGroup
        {
            get => _firstChildGroup;
            set
            {
                _firstChildGroup = value;
                _firstChildGroup.ParentGroup = this;
            }
        }

        public DockablePaneGroup SecondChildGroup
        {
            get => _secondChildGroup;
            set
            {
                _secondChildGroup = value;
                _secondChildGroup.ParentGroup = this;
            }
        }

        private DockablePaneGroup _firstChildGroup;
        private DockablePaneGroup _secondChildGroup;

        /// <summary>
        /// Needed only for deserialization
        /// </summary>
        public DockablePaneGroup() { }

        /// <summary>
        /// Create a group with a single pane
        /// </summary>
        /// <param name="pane">Attached pane</param>
        public DockablePaneGroup(Pane pane)
        {
            AttachedPane = pane;
        }

        /// <summary>
        /// Create a group with no panes
        /// </summary>
        public DockablePaneGroup(DockablePaneGroup firstGroup, DockablePaneGroup secondGroup, Docks groupDock)
        {
            FirstChildGroup = firstGroup;
            SecondChildGroup = secondGroup;
            Dock = groupDock;
        }

        public Pane GetPaneFromContent(DockableContent content)
        {
            if (AttachedPane != null && AttachedPane.Contents.Contains(content))
            {
                return AttachedPane;
            }
            else if (FirstChildGroup != null)
            {
                Pane pane = FirstChildGroup.GetPaneFromContent(content);
                if (pane != null)
                {
                    return pane;
                }
            }

            if (SecondChildGroup != null)
            {
                return SecondChildGroup.GetPaneFromContent(content);
            }

            return null;
        }

        bool IsHidden
        {
            get 
            {
                if (AttachedPane != null)
                {
                    return AttachedPane.IsHidden;
                }
                else
                {
                    return FirstChildGroup.IsHidden && SecondChildGroup.IsHidden;
                }
            }
        }

        GridLength GroupWidth
        {
            get
            {
                if (AttachedPane != null)
                {
                    return new GridLength(AttachedPane.PaneWidth, GridUnitType.Pixel);
                }
                else
                {
                    if (Dock == Docks.Left || Dock == Docks.Right)
                    {
                        return new GridLength(FirstChildGroup.GroupWidth.Value + SecondChildGroup.GroupWidth.Value + 4, GridUnitType.Pixel);
                    }
                    else
                    {
                        return FirstChildGroup.GroupWidth;
                    }  
                }
            }
        }

        GridLength GroupHeight
        {
            get
            {
                if (AttachedPane != null)
                {
                    return new GridLength(AttachedPane.PaneHeight, GridUnitType.Pixel);
                } 
                else
                {
                    if (Dock == Docks.Top || Dock == Docks.Bottom)
                    {
                        return new GridLength(FirstChildGroup.GroupHeight.Value + SecondChildGroup.GroupHeight.Value + 4, GridUnitType.Pixel);
                    } 
                    else
                    {
                        return FirstChildGroup.GroupHeight;
                    }
                }
            }
        }

        public void Arrange(Grid grid)
        {
            if (AttachedPane != null && AttachedPane.Parent == null)//AttachedPane.IsHidden)
            {
                grid.Children.Add(AttachedPane);
            }
            else if (FirstChildGroup != null && SecondChildGroup != null && FirstChildGroup.IsHidden && !SecondChildGroup.IsHidden)
            {
                SecondChildGroup.Arrange(grid);
            }
            else if (FirstChildGroup != null && SecondChildGroup != null && !FirstChildGroup.IsHidden && SecondChildGroup.IsHidden)
            {
                FirstChildGroup.Arrange(grid);
            }
            else
            {
                grid.RowDefinitions.Add(new RowDefinition()
                {
                    Height = new GridLength(1, GridUnitType.Star) 
                });
                grid.ColumnDefinitions.Add(new ColumnDefinition()
                {
                    Width = new GridLength(1, GridUnitType.Star)
                });

                if (Dock == Docks.None)
                {
                    var firstChildGrid = new Grid()
                    {
                        Margin = new Thickness(0, 0, 4, 0)
                    };
                    firstChildGrid.SetValue(Grid.ColumnProperty, 0);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);
                }

                if (Dock.HasFlag(Docks.Bottom) && Dock.HasFlag(Docks.Right))
                {
                    var firstChildGrid = new Grid()
                    {
                        Margin = new Thickness(0, 0, 4, 0)
                    };
                    firstChildGrid.SetValue(Grid.ColumnProperty, 0);
                    FirstChildGroup.Arrange(firstChildGrid);

                    var child = firstChildGrid.Children
                            .Cast<UIElement>()
                            .First(e => Grid.GetRow(e) == firstChildGrid.RowDefinitions.Count - 1
                                && Grid.GetColumn(e) == firstChildGrid.ColumnDefinitions.Count - 1);
                    var childGrid = child as Grid;

                    if (firstChildGrid.RowDefinitions.Count > firstChildGrid.ColumnDefinitions.Count)
                    {
                        childGrid.RowDefinitions.Add(new RowDefinition());
                        childGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        
                        childGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        childGrid.ColumnDefinitions[childGrid.ColumnDefinitions.Count - 1].Width = new GridLength(0, GridUnitType.Auto);

                        var splitter = new GridSplitter
                        {
                            Width = 4,
                            HorizontalAlignment = HorizontalAlignment.Right,
                            VerticalAlignment = VerticalAlignment.Stretch,
                            ResizeBehavior = GridResizeBehavior.PreviousAndNext
                        };
                        splitter.SetValue(Grid.ColumnProperty, childGrid.ColumnDefinitions.Count - 1);
                        childGrid.Children.Add(splitter);

                        childGrid.ColumnDefinitions.Add(new ColumnDefinition());
                        SecondChildGroup.AttachedPane.SetValue(Grid.ColumnProperty, childGrid.ColumnDefinitions.Count - 1);
                        SecondChildGroup.AttachedPane.SetValue(Grid.RowProperty, childGrid.RowDefinitions.Count - 1);

                        childGrid.Children.Add(SecondChildGroup.AttachedPane);
                    }

                    grid.Children.Add(firstChildGrid);
                    return;
                }

                if (Dock.HasFlag(Docks.Left))
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);// FirstChildGroup.GroupWidth;
                    grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

                    var firstChildGrid = new Grid()
                    {
                        Margin = new Thickness(0, 0, 4, 0)
                    };
                    firstChildGrid.SetValue(Grid.ColumnProperty, 0);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);

                    var secondChildGrid = new Grid();
                    secondChildGrid.SetValue(Grid.ColumnProperty, 1);
                    //secondChildGrid.Margin = (Dock == Dock.Right) ? new Thickness(0, 0, 4, 0) : new Thickness();
                    SecondChildGroup.Arrange(secondChildGrid);
                    grid.Children.Add(secondChildGrid);

                    var splitter = new GridSplitter
                    {
                        Width = 4,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    grid.Children.Add(splitter);
                }

                if (Dock.HasFlag(Docks.Top))
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    //grid.RowDefinitions[0].Height = (Dock == Dock.Top) ? new GridLength(AttachedPane.PaneHeight) : new GridLength(1, GridUnitType.Star);
                    //grid.RowDefinitions[1].Height = (Dock == Dock.Bottom) ? new GridLength(AttachedPane.PaneHeight) : new GridLength(1, GridUnitType.Star);
                    grid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);// FirstChildGroup.GroupHeight;
                    grid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star);

                    grid.RowDefinitions[0].MinHeight = 50;
                    grid.RowDefinitions[1].MinHeight = 50;

                    var firstChildGrid = new Grid();
                    //firstChildGrid.SetValue(Grid.RowProperty, (Dock == Dock.Bottom) ? 1 : 0);
                    firstChildGrid.SetValue(Grid.RowProperty, 0);
                    //firstChildGrid.Margin = (Dock == Dock.Bottom) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    firstChildGrid.Margin = new Thickness(0, 0, 0, 4);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);

                    var secondChildGrid = new Grid();
                    //secondChildGrid.SetValue(Grid.RowProperty, (Dock == Dock.Top) ? 1 : 0);
                    secondChildGrid.SetValue(Grid.RowProperty, 1);
                    //secondChildGrid.Margin = (Dock == Dock.Bottom) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    SecondChildGroup.Arrange(secondChildGrid);
                    grid.Children.Add(secondChildGrid);

                    //AttachedPane.SetValue(Grid.RowProperty, (Dock == Dock.Bottom) ? 1 : 0);
                    //AttachedPane.Margin = (Dock == Dock.Top) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    //grid.Children.Add(AttachedPane);

                    var splitter = new GridSplitter
                    {
                        Height = 4,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };
                    grid.Children.Add(splitter);
                }

                if (Dock.HasFlag(Docks.Right))
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.ColumnDefinitions[0].Width = new GridLength(1, GridUnitType.Star);
                    grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star); //SecondChildGroup.GroupWidth;

                    var firstChildGrid = new Grid()
                    {
                        Margin = new Thickness(0, 0, 4, 0)
                    };
                    firstChildGrid.SetValue(Grid.ColumnProperty, 0);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);

                    var secondChildGrid = new Grid();
                    secondChildGrid.SetValue(Grid.ColumnProperty, 1);
                    //secondChildGrid.Margin = (Dock == Dock.Right) ? new Thickness(0, 0, 4, 0) : new Thickness();
                    SecondChildGroup.Arrange(secondChildGrid);
                    grid.Children.Add(secondChildGrid);

                    var splitter = new GridSplitter
                    {
                        Width = 4,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    grid.Children.Add(splitter);
                }

                if (Dock.HasFlag(Docks.Bottom))
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    //grid.RowDefinitions[0].Height = (Dock == Dock.Top) ? new GridLength(AttachedPane.PaneHeight) : new GridLength(1, GridUnitType.Star);
                    //grid.RowDefinitions[1].Height = (Dock == Dock.Bottom) ? new GridLength(AttachedPane.PaneHeight) : new GridLength(1, GridUnitType.Star);
                    grid.RowDefinitions[0].Height = new GridLength(1, GridUnitType.Star);
                    grid.RowDefinitions[1].Height = new GridLength(1, GridUnitType.Star); //SecondChildGroup.GroupHeight;

                    grid.RowDefinitions[0].MinHeight = 50;
                    grid.RowDefinitions[1].MinHeight = 50;

                    var firstChildGrid = new Grid();
                    //firstChildGrid.SetValue(Grid.RowProperty, (Dock == Dock.Bottom) ? 1 : 0);
                    firstChildGrid.SetValue(Grid.RowProperty, 0);
                    //firstChildGrid.Margin = (Dock == Dock.Bottom) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    firstChildGrid.Margin = new Thickness(0, 0, 0, 4);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);

                    var secondChildGrid = new Grid();
                    //secondChildGrid.SetValue(Grid.RowProperty, (Dock == Dock.Top) ? 1 : 0);
                    secondChildGrid.SetValue(Grid.RowProperty, 1);
                    //secondChildGrid.Margin = (Dock == Dock.Bottom) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    SecondChildGroup.Arrange(secondChildGrid);
                    grid.Children.Add(secondChildGrid);

                    //AttachedPane.SetValue(Grid.RowProperty, (Dock == Dock.Bottom) ? 1 : 0);
                    //AttachedPane.Margin = (Dock == Dock.Top) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    //grid.Children.Add(AttachedPane);

                    var splitter = new GridSplitter
                    {
                        Height = 4,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };
                    grid.Children.Add(splitter);
                }

                /*if (Dock == Docks.Left || Dock == Docks.Right)
                {
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    //grid.ColumnDefinitions[0].Width = (Dock == Dock.Left) ? new GridLength(AttachedPane.PaneWidth) : new GridLength(1, GridUnitType.Star);
                    //grid.ColumnDefinitions[1].Width = (Dock == Dock.Right) ? new GridLength(AttachedPane.PaneWidth) : new GridLength(1, GridUnitType.Star);
                    grid.ColumnDefinitions[0].Width = (Dock == Docks.Left) ? FirstChildGroup.GroupWidth : new GridLength(1, GridUnitType.Star);
                    grid.ColumnDefinitions[1].Width = (Dock == Docks.Right) ? SecondChildGroup.GroupWidth : new GridLength(1, GridUnitType.Star);

                    var firstChildGrid = new Grid();
                    firstChildGrid.SetValue(Grid.ColumnProperty, 0);
                    firstChildGrid.Margin = new Thickness(0, 0, 4, 0);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);

                    var secondChildGrid = new Grid();
                    secondChildGrid.SetValue(Grid.ColumnProperty, 1);
                    //secondChildGrid.Margin = (Dock == Dock.Right) ? new Thickness(0, 0, 4, 0) : new Thickness();
                    SecondChildGroup.Arrange(secondChildGrid);
                    grid.Children.Add(secondChildGrid);

                    var splitter = new GridSplitter
                    {
                        Width = 4,
                        HorizontalAlignment = HorizontalAlignment.Right,
                        VerticalAlignment = VerticalAlignment.Stretch
                    };
                    grid.Children.Add(splitter);
                }
                else // if (Dock == Dock.Top || Dock == Dock.Bottom)
                {
                    grid.ColumnDefinitions.Add(new ColumnDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    grid.RowDefinitions.Add(new RowDefinition());
                    //grid.RowDefinitions[0].Height = (Dock == Dock.Top) ? new GridLength(AttachedPane.PaneHeight) : new GridLength(1, GridUnitType.Star);
                    //grid.RowDefinitions[1].Height = (Dock == Dock.Bottom) ? new GridLength(AttachedPane.PaneHeight) : new GridLength(1, GridUnitType.Star);
                    grid.RowDefinitions[0].Height = (Dock == Docks.Top) ? FirstChildGroup.GroupHeight : new GridLength(1, GridUnitType.Star);
                    grid.RowDefinitions[1].Height = (Dock == Docks.Bottom) ? SecondChildGroup.GroupHeight : new GridLength(1, GridUnitType.Star);
                    
                    grid.RowDefinitions[0].MinHeight = 50;
                    grid.RowDefinitions[1].MinHeight = 50;

                    var firstChildGrid = new Grid();
                    //firstChildGrid.SetValue(Grid.RowProperty, (Dock == Dock.Bottom) ? 1 : 0);
                    firstChildGrid.SetValue(Grid.RowProperty, 0);
                    //firstChildGrid.Margin = (Dock == Dock.Bottom) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    firstChildGrid.Margin = new Thickness(0, 0, 0, 4);
                    FirstChildGroup.Arrange(firstChildGrid);
                    grid.Children.Add(firstChildGrid);

                    var secondChildGrid = new Grid();
                    //secondChildGrid.SetValue(Grid.RowProperty, (Dock == Dock.Top) ? 1 : 0);
                    secondChildGrid.SetValue(Grid.RowProperty, 1);
                    //secondChildGrid.Margin = (Dock == Dock.Bottom) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    SecondChildGroup.Arrange(secondChildGrid);
                    grid.Children.Add(secondChildGrid);

                    //AttachedPane.SetValue(Grid.RowProperty, (Dock == Dock.Bottom) ? 1 : 0);
                    //AttachedPane.Margin = (Dock == Dock.Top) ? new Thickness(0, 0, 0, 4) : new Thickness();
                    //grid.Children.Add(AttachedPane);

                    var splitter = new GridSplitter
                    {
                        Height = 4,
                        HorizontalAlignment = HorizontalAlignment.Stretch,
                        VerticalAlignment = VerticalAlignment.Bottom
                    };
                    grid.Children.Add(splitter);
                }*/
            }
        }

        ///// <summary>
        ///// Arrange passed grid layout
        ///// </summary>
        ///// <param name="grid">Grid to arrange</param>
        ///// <param name="addPaneToGrid">If true add atteched panes to the grid</param>
        ///// <remarks>Setting <paramref name="addToPaneToGrid"/> to false, this functions only arrange grids layout, without 
        ///// appending attached pane to grid children collection. This is useful for dragging operations.</remarks>
        //public void Arrange(Grid grid, bool addPaneToGrid)
        //{
        //    if (AttachedPane != null)
        //    {
        //        if (addPaneToGrid)
        //            grid.Children.Add(AttachedPane);
        //    }
        //    else
        //    {
        //        if (Group1.IsHidden && !Group2.IsHidden)
        //            Group2.Arrange(grid, addPaneToGrid);
        //        else if (!Group1.IsHidden && Group2.IsHidden)
        //            Group1.Arrange(grid, addPaneToGrid);
        //        else
        //        {
        //                //first child grid
        //                Grid grid1 = new Grid();
        //                //..and second one
        //                Grid grid2 = new Grid();                    
                
        //            #region Vertical orientation
        //            if (SplitOrientation == SplitOrientation.Vertical)
        //            {
        //                 //only one row
        //                grid.RowDefinitions.Add(new RowDefinition());
        //                //two cols
        //                grid.ColumnDefinitions.Add(new ColumnDefinition());
        //                grid.ColumnDefinitions.Add(new ColumnDefinition());

        //                //setup widths
        //                grid.ColumnDefinitions[0].MinWidth = 50;
        //                grid.ColumnDefinitions[0].Width = Group1.GridWidth;
        //                grid.ColumnDefinitions[1].MinWidth = 50;
        //                grid.ColumnDefinitions[1].Width = Group2.GridWidth;

        //                //ensure that at least one col has star length
        //                if (!grid.ColumnDefinitions[0].Width.IsStar &&
        //                    !grid.ColumnDefinitions[1].Width.IsStar)
        //                    grid.ColumnDefinitions[1].Width = new GridLength(1, GridUnitType.Star);

        //                grid1.SetValue(Grid.ColumnProperty, 0);
        //                grid2.SetValue(Grid.ColumnProperty, 1);

        //                GridSplitter splitter = new GridSplitter();
        //                splitter.VerticalAlignment = VerticalAlignment.Stretch;
        //                splitter.HorizontalAlignment = HorizontalAlignment.Left;
        //                splitter.SetValue(Grid.ColumnProperty, 1);
        //                splitter.SetValue(Grid.RowProperty, 0);
        //                splitter.Width = 5;
        //                //make room for the splitter
        //                grid2.Margin = new Thickness(5,0,0,0);

        //                //ok, now add child grids and a splitter between them to current grid
        //                grid.Children.Add(grid1);
        //                grid.Children.Add(splitter);
        //                grid.Children.Add(grid2);

        //                //finally arrange child grids
        //                Group1.Arrange(grid1, addPaneToGrid);
        //                Group2.Arrange(grid2, addPaneToGrid);
        //            }
        //            #endregion

        //            #region Horizontal Orientation
        //            else //if (SplitOrientation == SplitOrientation.Horizontal)
        //            {
        //            }
        //            #endregion
        //        }
        //    }

        //}

        //DockPanel GetChildElement(DockablePaneGroup group, Dock dock)
        //{
        //    DockPanel childPanel = new DockPanel();
        //    childPanel.SetValue(DockPanel.DockProperty, dock);
            
        //    if (SplitOrientation == SplitOrientation.Vertical)
        //        childPanel.Width = group.DockPanelWidth;
        //    else
        //        childPanel.Height = group.DockPanelHeight;

        //    group.Arrange(childPanel);

        //    //childPanels.Add(childPanel);
        //    return childPanel;
        //}

        //internal void Arrange(DockPanel panel)
        //{

        //    if (AttachedPane != null)
        //    {
        //        AttachedPane.AttachPanel(panel);
        //        panel.Children.Add(AttachedPane);
        //    }
        //    else
        //    {
        //        #region Vertical split
        //        if (SplitOrientation == SplitOrientation.Vertical)
        //        {
        //            DockPanel lastPanel = null;
                    
        //            foreach (DockablePaneGroup group in ChildGroups)
        //            {
        //                if (group.IsHidden)
        //                    continue;

        //                if (double.IsNaN(group.DockPanelWidth))
        //                {
        //                    lastPanel = GetChildElement(group, Dock.Left);
        //                }
        //                else
        //                {
        //                    DockPanel panelToAdd = GetChildElement(group, lastPanel == null ? Dock.Left : Dock.Right);
        //                    panel.Children.Add(panelToAdd);
        //                }
        //            }

        //            if (lastPanel != null)
        //                panel.Children.Add(lastPanel);

        //            Dock currentDock = Dock.Left;

        //            for (int i = 0; i < panel.Children.Count-1; i++)
        //            {
        //                currentDock = (Dock)panel.Children[i].GetValue(DockPanel.DockProperty);
        //                DockPanel prevPanel = panel.Children[i] as DockPanel;
        //                DockPanel nextPanel = null;
        //                for (int j = i+1; j < panel.Children.Count; j++)
        //                    if ((Dock)panel.Children[j].GetValue(DockPanel.DockProperty) ==
        //                        currentDock)
        //                    {
        //                        nextPanel = panel.Children[j] as DockPanel;
        //                        break;
        //                    }
        //                if (nextPanel == null)
        //                    nextPanel = panel.Children[panel.Children.Count - 1] as DockPanel;

        //                DockPanelSplitter splitter = null;
                        
        //                if (currentDock == Dock.Left)
        //                    splitter = new DockPanelSplitter(prevPanel, nextPanel, SplitOrientation.Vertical);
        //                else
        //                    splitter = new DockPanelSplitter(nextPanel, prevPanel, SplitOrientation.Vertical);
        //                splitter.SetValue(DockPanel.DockProperty, currentDock);
                        
        //                i++;
        //                panel.Children.Insert(i, splitter);
        //            }
        //        }
        //        #endregion
        //        #region Horizontal split
        //        else //if (SplitOrientation == SplitOrientation.Vertical)
        //        {
        //            DockPanel lastPanel = null;

        //            foreach (DockablePaneGroup group in ChildGroups)
        //            {
        //                if (group.IsHidden)
        //                    continue;

        //                if (double.IsNaN(group.DockPanelWidth))
        //                {
        //                    lastPanel = GetChildElement(group, Dock.Top);
        //                }
        //                else
        //                {
        //                    DockPanel panelToAdd = GetChildElement(group, lastPanel == null ? Dock.Top : Dock.Bottom);
        //                    panel.Children.Add(panelToAdd);
        //                }
        //            }

        //            if (lastPanel != null)
        //                panel.Children.Add(lastPanel);

        //            Dock currentDock = Dock.Top;

        //            for (int i = 0; i < panel.Children.Count - 1; i++)
        //            {
        //                currentDock = (Dock)panel.Children[i].GetValue(DockPanel.DockProperty);
        //                DockPanel prevPanel = panel.Children[i] as DockPanel;
        //                DockPanel nextPanel = null;
        //                for (int j = i + 1; j < panel.Children.Count; j++)
        //                    if ((Dock)panel.Children[j].GetValue(DockPanel.DockProperty) ==
        //                        currentDock)
        //                    {
        //                        nextPanel = panel.Children[j] as DockPanel;
        //                        break;
        //                    }
        //                if (nextPanel == null)
        //                    nextPanel = panel.Children[panel.Children.Count - 1] as DockPanel;

        //                DockPanelSplitter splitter = null;

        //                if (currentDock == Dock.Top)
        //                    splitter = new DockPanelSplitter(prevPanel, nextPanel, SplitOrientation.Horizontal);
        //                else
        //                    splitter = new DockPanelSplitter(nextPanel, prevPanel, SplitOrientation.Horizontal);
        //                splitter.SetValue(DockPanel.DockProperty, currentDock);

        //                i++;
        //                panel.Children.Insert(i, splitter);
        //            }
        //        }
        //        #endregion
        //    }
        //}

        public void ReplaceChildGroup(DockablePaneGroup groupToFind, DockablePaneGroup groupToReplace)
        {
            if (FirstChildGroup == groupToFind)
                FirstChildGroup = groupToReplace;
            else if (SecondChildGroup == groupToFind)
                SecondChildGroup = groupToReplace;
            else
            {
                System.Diagnostics.Debug.Assert(false);
            }
        }

        //public void SaveChildPanesSize()
        //{
        //    if (AttachedPane != null && ParentGroup!=null)
        //        AttachedPane.SaveSize(ParentGroup.Dock);
        //    else
        //    {
        //        FirstChildGroup.SaveChildPanesSize();
        //        SecondChildGroup.SaveChildPanesSize();
        //    }
        //}

        public DockablePaneGroup AddPane(DockablePane pane)
        {
            if (pane.Dock.HasFlag(Docks.Left) || pane.Dock.HasFlag(Docks.Top))
            {
                return new DockablePaneGroup(new DockablePaneGroup(pane), this, pane.Dock);
            }

            if (pane.Dock.HasFlag(Docks.Right) || pane.Dock.HasFlag(Docks.Bottom))
            {
                return new DockablePaneGroup(this, new DockablePaneGroup(pane), pane.Dock);
            }

            if (pane.Dock == Docks.None)
            {
                return new DockablePaneGroup(pane);
            }

            return null;
            //DockablePaneGroup resGroup = null;

            //if (AttachedPane != null)
            //{
            //    DockablePaneGroup newChildGroup = new DockablePaneGroup(AttachedPane);
            //    switch (pane.Dock)
            //    {
            //        case Dock.Left:
            //            resGroup = new DockablePaneGroup(new DockablePaneGroup(pane), newChildGroup, SplitOrientation.Vertical);
            //            break;
            //        case Dock.Right:
            //            resGroup = new DockablePaneGroup(newChildGroup, new DockablePaneGroup(pane), SplitOrientation.Vertical);
            //            break;
            //        case Dock.Top:
            //            resGroup = new DockablePaneGroup(new DockablePaneGroup(pane), newChildGroup, SplitOrientation.Horizontal);
            //            break;
            //        case Dock.Bottom:
            //            resGroup = new DockablePaneGroup(newChildGroup, new DockablePaneGroup(pane), SplitOrientation.Horizontal);
            //            break;
            //    }
            //}
            //else
            //{
            //    if (SplitOrientation == SplitOrientation.Vertical)
            //    {
            //        if (pane.Dock == Dock.Left)
            //        {
            //            ChildGroups.Insert(0, new DockablePaneGroup(pane));
            //            resGroup = this;
            //        }
            //        else if (pane.Dock == Dock.Right)
            //        {
            //            int index = 0; 
            //            for (int i = 0; i < ChildGroups.Count;i++)
            //                if (ChildGroups[i].do
                            
            //            ChildGroups.Add(new DockablePaneGroup(pane));
            //            resGroup = this;
            //        }
            //        else if (pane.Dock == Dock.Bottom)
            //            resGroup = new DockablePaneGroup(this, new DockablePaneGroup(pane), SplitOrientation.Horizontal);
            //        else if (pane.Dock == Dock.Top)
            //            resGroup = new DockablePaneGroup(new DockablePaneGroup(pane), this, SplitOrientation.Horizontal);
            //    }
            //    else //if (SplitOrientation == SplitOrientation.Horizontal)
            //    {
            //        if (pane.Dock == Dock.Top)
            //        {
            //            ChildGroups.Insert(0, new DockablePaneGroup(pane));
            //            resGroup = this;
            //        }
            //        else if (pane.Dock == Dock.Bottom)
            //        {
            //            ChildGroups.Add(new DockablePaneGroup(pane));
            //            resGroup = this;
            //        }
            //        else if (pane.Dock == Dock.Right)
            //            resGroup = new DockablePaneGroup(this, new DockablePaneGroup(pane), SplitOrientation.Vertical);
            //        else if (pane.Dock == Dock.Left)
            //            resGroup = new DockablePaneGroup(new DockablePaneGroup(pane), this, SplitOrientation.Vertical);
            //    }
            //}
            
            //return resGroup;
        }
  
        public DockablePaneGroup RemovePane(DockablePane pane)
        {
            if (AttachedPane != null)
            {
                return null;
            }

            if (FirstChildGroup.AttachedPane==pane)
            {
                return SecondChildGroup;
            }
            else if (SecondChildGroup.AttachedPane==pane)
            {
                return FirstChildGroup;
            }
            else
            {
                DockablePaneGroup group = FirstChildGroup.RemovePane(pane);

                if (group != null)
                {
                    FirstChildGroup = group;
                    group.ParentGroup = this;
                    return null;
                }

                group = SecondChildGroup.RemovePane(pane);

                if (group != null)
                {
                    SecondChildGroup = group;
                    group.ParentGroup = this;
                    return null;
                }
            }

            return null;
        }

        public DockablePaneGroup GetPaneGroup(Pane pane)
        {
            if (AttachedPane == pane)
            {
                return this;
            }
            else
            {
                if (FirstChildGroup != null)
                {
                    DockablePaneGroup paneGroup = FirstChildGroup.GetPaneGroup(pane);
                    if (paneGroup != null)
                    {
                        return paneGroup;
                    }
                }
                if (SecondChildGroup != null)
                {
                    DockablePaneGroup paneGroup = SecondChildGroup.GetPaneGroup(pane);
                    if (paneGroup != null)
                    {
                        return paneGroup;
                    }
                }

                return null;
            }
        }

        #region ILayoutSerializable Membri di

        public void Serialize(XmlDocument doc, XmlNode parentNode)
        {
            parentNode.Attributes.Append(doc.CreateAttribute("Dock"));
            parentNode.Attributes["Dock"].Value = Dock.ToString();

            if (AttachedPane != null)
            {
                XmlNode nodeAttachedPane = null;

                if (AttachedPane is DockablePane)
                {
                    nodeAttachedPane = doc.CreateElement("DockablePane");
                }
                else if (AttachedPane is DocumentsPane)
                {
                    nodeAttachedPane = doc.CreateElement("DocumentsPane");
                }

                AttachedPane.Serialize(doc, nodeAttachedPane);

                parentNode.AppendChild(nodeAttachedPane);
            }
            else
            {
                XmlNode nodeChildGroups = doc.CreateElement("ChildGroups");

                XmlNode nodeFirstChildGroup = doc.CreateElement("FirstChildGroup");
                FirstChildGroup.Serialize(doc, nodeFirstChildGroup);
                nodeChildGroups.AppendChild(nodeFirstChildGroup);

                XmlNode nodeSecondChildGroup = doc.CreateElement("SecondChildGroup");
                SecondChildGroup.Serialize(doc, nodeSecondChildGroup);
                nodeChildGroups.AppendChild(nodeSecondChildGroup);

                parentNode.AppendChild(nodeChildGroups);
            }
        }

        public void Deserialize(DockManager managerToAttach, System.Xml.XmlNode node, GetContentFromTypeString getObjectHandler)
        {
            Dock = (Docks)Enum.Parse(typeof(Docks), node.Attributes["Dock"].Value);

            if (node.ChildNodes[0].Name == "DockablePane")
            {
                var pane = new DockablePane(managerToAttach);
                pane.Deserialize(managerToAttach, node.ChildNodes[0], getObjectHandler);
                AttachedPane = pane;
            }
            else if (node.ChildNodes[0].Name == "DocumentsPane")
            {
                DocumentsPane pane = managerToAttach.GetDocumentsPane();
                pane.Deserialize(managerToAttach, node.ChildNodes[0], getObjectHandler);
                AttachedPane = pane;
            }
            else
            {
                _firstChildGroup = new DockablePaneGroup
                {
                    ParentGroup = this
                };
                _firstChildGroup.Deserialize(managerToAttach, node.ChildNodes[0].ChildNodes[0], getObjectHandler);

                _secondChildGroup = new DockablePaneGroup
                {
                    ParentGroup = this
                };
                _secondChildGroup.Deserialize(managerToAttach, node.ChildNodes[0].ChildNodes[1], getObjectHandler);
            }
        }

        #endregion
    }
}
