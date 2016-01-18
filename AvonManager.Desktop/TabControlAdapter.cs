using System;
using System.Net;
using System.Windows;
using Microsoft.Practices.Prism.Regions;
using System.Linq;
using System.Windows.Controls;
using System.Windows.Data;

namespace  AvonManager.Desktop
{
	public class TabControlAdapter : RegionAdapterBase<TabControl>
	{
		public TabControlAdapter(IRegionBehaviorFactory regionBehaviorFactory)
			: base(regionBehaviorFactory)
		{ }

		protected override void Adapt(IRegion region, TabControl regionTarget)
		{
			region.ActiveViews.CollectionChanged += (sender, args) =>
			{
				switch (args.Action)
				{
					case System.Collections.Specialized.NotifyCollectionChangedAction.Add:
					foreach (UserControl view in args.NewItems)
					{
						TabItem tab = new TabItem();
						tab.DataContext = view.DataContext;
                        tab.Header = view.Name;
						tab.Style = regionTarget.ItemContainerStyle;
						tab.Content = view;
						regionTarget.Items.Add(tab);
					}
					break;
					case System.Collections.Specialized.NotifyCollectionChangedAction.Remove:
					foreach (UserControl view in args.OldItems)
					{
						TabItem viewTab = regionTarget.Items.Cast<TabItem>().Single(o => o.DataContext == view.DataContext);
						regionTarget.Items.Remove(viewTab);
					}
					break;

				}
			};
		}

		protected override IRegion CreateRegion()
		{
			return new AllActiveRegion();
		}
	}
}
