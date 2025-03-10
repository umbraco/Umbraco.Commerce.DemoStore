//------------------------------------------------------------------------------
// <auto-generated>
//   This code was generated by a tool.
//
//    Umbraco.ModelsBuilder.Embedded v15.0.0--rc1.preview.16+7523e47
//
//   Changes to this file will be lost if the code is regenerated.
// </auto-generated>
//------------------------------------------------------------------------------

using System;
using System.Linq.Expressions;
using Umbraco.Cms.Core.Models.PublishedContent;
using Umbraco.Cms.Core.PublishedCache;
using Umbraco.Cms.Infrastructure.ModelsBuilder;
using Umbraco.Cms.Core;
using Umbraco.Extensions;

namespace Umbraco.Commerce.DemoStore.Models
{
	/// <summary>Home Page</summary>
	[PublishedModel("homePage")]
	public partial class HomePage : Page
	{
		// helpers
#pragma warning disable 0109 // new is redundant
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		public new const string ModelTypeAlias = "homePage";
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		public new const PublishedItemType ModelItemType = PublishedItemType.Content;
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public new static IPublishedContentType GetModelContentType(IPublishedContentTypeCache contentTypeCache)
			=> PublishedModelUtility.GetModelContentType(contentTypeCache, ModelItemType, ModelTypeAlias);
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[return: global::System.Diagnostics.CodeAnalysis.MaybeNull]
		public static IPublishedPropertyType GetModelPropertyType<TValue>(IPublishedContentTypeCache contentTypeCache, Expression<Func<HomePage, TValue>> selector)
			=> PublishedModelUtility.GetModelPropertyType(GetModelContentType(contentTypeCache), selector);
#pragma warning restore 0109

		private IPublishedValueFallback _publishedValueFallback;

		// ctor
		public HomePage(IPublishedContent content, IPublishedValueFallback publishedValueFallback)
			: base(content, publishedValueFallback)
		{
			_publishedValueFallback = publishedValueFallback;
		}

		// properties

		///<summary>
		/// Featured Products: Select products to feature on the home page.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("featuredProducts")]
		public virtual global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent> FeaturedProducts => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent>>(_publishedValueFallback, "featuredProducts");

		///<summary>
		/// Description: A description for the site, to be used in taglines and auto-generated page titles.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteDescription")]
		public virtual string SiteDescription => this.Value<string>(_publishedValueFallback, "siteDescription");

		///<summary>
		/// Footer Links: The links to display in the very foot of the site. Usually a mix of helpful links + any legally required pages such as Terms and Conditions and Privacy Policy.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteFooterLinks")]
		public virtual global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent> SiteFooterLinks => this.Value<global::System.Collections.Generic.IEnumerable<global::Umbraco.Cms.Core.Models.PublishedContent.IPublishedContent>>(_publishedValueFallback, "siteFooterLinks");

		///<summary>
		/// Name: A name for the site, to be used in taglines and auto-generated page titles.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteName")]
		public virtual string SiteName => this.Value<string>(_publishedValueFallback, "siteName");

		///<summary>
		/// Raw Markup: DANGEROUS! Drop in any raw markup to be appended before the closing body tag across ALL pages of the site. Used mostly for analytics code snippets. It's best not to use this field unless you know what you are doing.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("siteRawMarkup")]
		public virtual string SiteRawMarkup => this.Value<string>(_publishedValueFallback, "siteRawMarkup");

		///<summary>
		/// Store: Select the store associated with this site.
		///</summary>
		[global::System.CodeDom.Compiler.GeneratedCodeAttribute("Umbraco.ModelsBuilder.Embedded", "15.0.0--rc1.preview.16+7523e47")]
		[global::System.Diagnostics.CodeAnalysis.MaybeNull]
		[ImplementPropertyType("store")]
		public virtual global::Umbraco.Commerce.Core.Models.StoreReadOnly Store => this.Value<global::Umbraco.Commerce.Core.Models.StoreReadOnly>(_publishedValueFallback, "store");
	}
}
