using Gameapp.Data;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mime;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.NewtonsoftJson;
//using System.Text.Json;
using Newtonsoft.Json;
using Gameapp.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using Gameapp.Areas.Identity.Pages.Account;
using Gameapp.Models.ViewModels;
using System.IO;
using System.Drawing;
using Microsoft.AspNetCore.Hosting;
using System.Net.Mail;
using Microsoft.IdentityModel.Protocols;
using Gameapp.Migrations;
using Gameapp.ViewModels;
using System.Net.Http;
using System.Text;
using System.Text.Encodings.Web;
using Microsoft.AspNetCore.Identity.UI.Services;
using System.Net.Http.Headers;
using Microsoft.Extensions.Configuration;
using RestSharp;
using Newtonsoft.Json.Linq;
// For more information on enabling Web API for empty projects, visit http://go.microsoft.com/fwlink/?LinkID=397860

namespace Gameapp
{
	[Route("api/[controller]/[action]")]
	[ApiController]
	[Produces(MediaTypeNames.Application.Json)]

	public class MobileController : ControllerBase
	{

		private GamesContext _context;
		public Order order { get; set; }
		public Models.ViewModels.OrderModel ordersmodel { get; set; }
		static string token = "rLtt6JWvbUHDDhsZnfpAhpYk4dxYDQkbcPTyGaKp2TYqQgG7FGZ5Th_WD53Oq8Ebz6A53njUoo1w3pjU1D4vs_ZMqFiz_j0urb_BH9Oq9VZoKFoJEDAbRZepGcQanImyYrry7Kt6MnMdgfG5jn4HngWoRdKduNNyP4kzcp3mRv7x00ahkm9LAK7ZRieg7k1PDAnBIOG3EyVSJ5kK4WLMvYr7sCwHbHcu4A5WwelxYK0GMJy37bNAarSJDFQsJ2ZvJjvMDmfWwDVFEVe_5tOomfVNt6bOg9mexbGjMrnHBnKnZR1vQbBtQieDlQepzTZMuQrSuKn-t5XZM7V6fCW7oP-uXGX-sMOajeX65JOf6XVpk29DP6ro8WTAflCDANC193yof8-f5_EYY-3hXhJj7RBXmizDpneEQDSaSz5sFk0sV5qPcARJ9zGG73vuGFyenjPPmtDtXtpx35A-BVcOSBYVIWe9kndG3nclfefjKEuZ3m4jL9Gg1h2JBvmXSMYiZtp9MR5I6pvbvylU_PP5xJFSjVTIz7IQSjcVGO41npnwIxRXNRxFOdIUHn0tjQ-7LwvEcTXyPsHXcMD8WtgBh-wxR8aKX7WPSsT1O8d8reb2aR7K3rkV3K82K_0OgawImEpwSvp9MNKynEAJQS6ZHe_J_l77652xwPNxMRTMASk1ZsJL";

		public List<OrderItem> orderItem { get; set; }
		public PaymentMehod paymentMethod { get; set; }
		private readonly IEmailSender _emailSender;
		public HttpClient httpClient { get; set; }

		private object user;
		private readonly UserManager<ApplicationUser> _userManager;
		private readonly SignInManager<ApplicationUser> _signInManager;
		private readonly ApplicationDbContext _db;
		private readonly IWebHostEnvironment _hostEnvironment;
		private readonly IConfiguration _configuration;

		public MobileController(
			GamesContext context, UserManager<ApplicationUser> userManager,
			SignInManager<ApplicationUser> signInManager,
			ApplicationDbContext db, IEmailSender emailSender,
			IWebHostEnvironment HostEnvironment,
			IConfiguration configuration)
		{
			_context = context;
			_userManager = userManager;
			_signInManager = signInManager;
			_db = db;
			_hostEnvironment = HostEnvironment;
			_emailSender = emailSender;
			httpClient = new HttpClient();
			_configuration = configuration;
		}

		#region "SideMenu"
		//Page 1 :Select Country and language
		[HttpGet]
		public dynamic GetCategorgAndSubCategoryList()
		{

			var lst = _context.Category
				.Where(c => c.IsActive == true)
				.OrderByDescending(c => c.OrderIndex.HasValue)
				.ThenBy(c => c.CategoryId)
				.Select(c => new
				{
					CategoryId = c.CategoryId,
					CategoryTlar = c.CategoryTlar,
					CategoryTlen = c.CategoryTlen,
					CategoryPic = "/images/categories/" + c.CategoryPic,
					CategoryIcon = "/images/categories/" + c.CategoryIcon,
					OrderIndex = c.OrderIndex,
					SubCategory = c.SubCategory.ToList()
				});
			return new { Category = lst };

		}
		#endregion

		#region "Page1"
		//Page 1 :Select Country and language
		[HttpGet]
		public ActionResult<Object> GetCountryList()
		{

			var countrey = _context.Country.Where(c => c.IsActive == true).OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					c.CountryId,
					c.CountryTlar,
					c.CountryTlen,
					Pic = "/images/countries/" + c.Pic,
					c.OrderIndex
				});
			return new { Data = countrey };

		}
		[HttpGet]

		public ActionResult<Object> GetCountryList2()
		{

			var countrey = _context.Country.Where(c => c.IsActive == true).OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					c.CountryId,
					c.CountryTlar,
					c.CountryTlen,
					//Pic = "http://" + HttpContext.Request.Host + "/images/" + c.Pic,
					c.OrderIndex
				});
			//var objectToSerialize = new { Property1 = "value1", SubOjbect = new { SubObjectId = 1 } };

			var json = JsonConvert.SerializeObject(countrey, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });

			//string sRegex = json.Replace("\\", "");
			return Ok(new { Data = json });

		}
		[HttpGet]

		public ActionResult<Object> GetCountryList1()
		{

			var countrey = _context.Country.Where(c => c.IsActive == true).OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					c.CountryId,
					c.CountryTlar,
					c.CountryTlen,
					//Pic = "http://" + HttpContext.Request.Host + "/images/" + c.Pic,
					c.OrderIndex
				});
			//var objectToSerialize = new { Property1 = "value1", SubOjbect = new { SubObjectId = 1 } };

			var json = JsonConvert.SerializeObject(countrey, new JsonSerializerSettings { ContractResolver = new Newtonsoft.Json.Serialization.CamelCasePropertyNamesContractResolver() });


			json = json.Replace("\"{", "{");
			json = json.Replace("}\"", "}");
			json = json.Replace("\\", "");
			return Ok(json);

		}
		#endregion

		#region "Page2"
		//Page 2: Home Screen --> Slider
		[HttpGet]
		public dynamic GetSliderList(int country)
		{

			var lst = _context.Slider.Where(s => (s.IsActive == true) && (s.CountryId == country)).OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					c.SliderId,
					Pic = "http://" + HttpContext.Request.Host + "/images/" + c.Pic,
					c.OrderIndex,
					c.EntityId,
					c.SliderTypeId,
					c.SliderType.SliderTypeTlar,
					c.SliderType.SliderTypeTlen
				});
			return new { Data = lst };

		}

		//Page 2 & 11: Home Screen  --> Shops List
		[HttpGet]
		public dynamic GetShopList(int country)
		{
			var lst = _context
				.Shop
				.Include(s => s.Subscriptions)
				.ToList()
				.Where(s => (s.IsActive == true) && (s.CountryId == country) && IsShopHasValidSub(s.ShopId))
				.OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					ShopId = c.ShopId,
					ShopTlar = c.ShopTlar,
					ShopTlen = c.ShopTlen,
					Tele = c.Tele,
					Mobile = c.Mobile,
					Address = c.Address,
					ShopNo = c.ShopNo,
					OrderIndex = c.OrderIndex,
					Pic = "http://" + HttpContext.Request.Host + "/images/" + c.Pic,
					Banner = "http://" + HttpContext.Request.Host + "/images/" + c.Banner,
					RegisterDate = c.RegisterDate,

				});
			return new { Data = lst };

		}
		[HttpGet]
		public dynamic Banner(int country)
		{

			var lst = _context.Banner
				.Where(b => (b.IsActive == true) && (b.CountryId == country))
				.OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					BannerId = c.BannerId,
					Pic = "http://" + HttpContext.Request.Host + "/images/" + c.Pic,
					OrderIndex = c.OrderIndex,
					SliderTypeId = c.SliderTypeId,
					EntityId = c.EntityId
				});
			return new { Data = lst };

		}
		[HttpGet]
		public ActionResult<Object> GetCurreny()
		{

			var currencyList = _context.Curreny
				.Where(c => c.IsActive == true)
				.Select(c => new
				{
					CurrenyId = c.CurrenyId,
					CurrenyTlar = c.CurrenyTlar,
					CurrenyTlen = c.CurrenyTlen,
					CurrencyPic = "/images/Curreny" + c.CurrencyPic,
				});
			return new { Data = currencyList };

		}


		#endregion


		#region "Page3"

		#endregion

		//Page 6: Category --> Subcategory List
		#region "Page6"
		[HttpGet]
		public dynamic GetSubCategorgByCategoryID(int CategoryId)
		{

			var lst = _context.SubCategory
				.Where(c => c.CategoryId == CategoryId && c.IsActive == true)
				.OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					SubCategoryId = c.SubCategoryId,
					CategoryId = c.CategoryId,
					SubCategoryTlar = c.SubCategoryTlar,
					SubCategoryTlen = c.SubCategoryTlen,
					SubCategoryPic = c.SubCategoryPic,
					OrderIndex = c.OrderIndex,
					Shop = c.Shop
				});

			return new { Data = lst };

		}
		//Page 6: Subcategory --> Prodect List
		[HttpGet]
		public dynamic GetItemsForEachSubCategorg(int SubCategoryId, int country)
		{

			var lst = _context.Items
				.Include(s => s.Shop)
				.ToList()
				.Where(c => (c.SubCategoryId == SubCategoryId && c.IsActive == true) && (c.CountryId == country)
				&& IsShopHasValidSub(c.ShopId.Value))

				.OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					CategoryId = c.CategoryId,
					SubCategoryId = c.SubCategoryId,
					ItemId = c.ItemId,
					ShopId = c.ShopId,
					ItemName = c.ItemName,
					ItemNameAr = c.ItemNameAr,
					ItemDescriptionAr = c.ItemDescriptionAr,
					ItemImage = "/images/" + c.ItemImage,
					ItemDescription = c.ItemDescription,
					ItemPrice = c.ItemPrice,
					OrderIndex = c.OrderIndex,
					OutOfStock = c.OutOfStock,
					Shop = c.Shop,
					SubCategory = c.SubCategory
				});

			return new { Data = lst };

		}
		#endregion
		[HttpGet]
		public bool IsShopHasValidSub(int Shopid)
		{
			if (_context.Subscriptions.Any(c => c.ShopId == Shopid && (c.StartDate <= DateTime.Now && c.EndDate >= DateTime.Now)))
			{
				return true;
			}
			else
			{
				return false;
			}
		}
		[HttpGet]
		public ActionResult<Object> GetItemsForEachShop(int Shopid, int customerId, int country)
		{

			if (IsShopHasValidSub(Shopid))
			{

				var lst = _context.Items
					.Include(s => s.Shop)
					.Where(c => (c.ShopId == Shopid) && (c.CountryId == country))
					.OrderBy(c => c.OrderIndex)
					.Select(c =>
					new
					{
						ItemId = c.ItemId,
						CategoryId = c.CategoryId,
						SubCategoryId = c.SubCategoryId,
						ShopId =
						c.ShopId,
						ItemName = c.ItemName,
						ItemNameAr = c.ItemNameAr,
						ItemImage = "/images/" + c.ItemImage,
						ItemDescription = c.ItemDescription,
						ItemDescriptionAr = c.ItemDescriptionAr,
						ItemPrice = c.ItemPrice,
						OutOfStock = c.OutOfStock,
						OrderIndex = c.OrderIndex,
						Shop = c.Shop,
						IsFavourite = _context.WishList.FirstOrDefault(w => w.ItemId == c.ItemId && w.CustomerId == customerId) == null ? false : true
					})
					.ToList();

				return new { Data = lst };

			}
			else
			{
				return NotFound();
			}


		}
		[HttpGet]
		public async Task<IActionResult> GetrelatedListByCountryId([FromQuery] int CountryId, int itemId)
		{
			try
			{
				var related = await _context.CollectionItems.Where(c => c.ItemId == itemId).Select(c => c.ItemId).ToListAsync();
				var related2 = _context.Items.Where(c => related.Contains(c.ItemId)).Select(i => new
				{
					i.ItemId,
					i.CategoryId,
					i.SubCategoryId,
					i.ShopId,
					i.ItemName,
					i.ItemNameAr,
					i.ItemImage,
					i.ItemDescription,
					i.ItemDescriptionAr,
					i.ItemPrice,
					i.IsActive,
					i.OrderIndex,
					i.CustomerId,
					//itemImage = i.ItemImage,
					IsFav = true
				});

				return Ok(new { related2 });
			}
			catch (Exception)
			{

				return Ok(new { status = false, message = "Something went wrong" });
			}


		}
		[HttpGet]
		public ActionResult<Object> GetAllItemsForEachShop(int Shopid, int CategoryId, int countryId)
		{

			if (IsShopHasValidSub(Shopid))
			{

				var lst = _context.Items
					.Include(s => s.Shop)
					.Where(c => (c.ShopId == Shopid) && (c.CategoryId == CategoryId) && (c.CountryId == countryId))
					.OrderBy(c => c.OrderIndex)
					.Select(c =>
					new
					{
						ItemId = c.ItemId,
						CategoryId = c.CategoryId,
						SubCategoryId = c.SubCategoryId,
						ShopId =
						c.ShopId,
						ItemName = c.ItemName,
						ItemNameAr = c.ItemNameAr,
						ItemImage = "/images/" + c.ItemImage,
						ItemDescription = c.ItemDescription,
						ItemDescriptionAr = c.ItemDescriptionAr,
						ItemPrice = c.ItemPrice,
						OutOfStock = c.OutOfStock,
						OrderIndex = c.OrderIndex,
						Shop = c.Shop,

					})
					.ToList();

				return new { Data = lst };

			}
			else
			{
				return NotFound();
			}


		}
		//public dynamic GetSubcategoryiesForEachShop(int Shopid)
		//{

		//    var lst = _context.SubCategory.Where(c => c.ShopId == Shopid).OrderBy(c => c.OrderIndex).Select(c => new { ItemId = c.ItemId, CategoryId = c.CategoryId, SubCategoryId = c.SubCategoryId, ShopId = c.ShopId, ItemName = c.ItemName, ItemImage = c.ItemImage, ItemDescription = c.ItemDescription, ItemPrice = c.ItemPrice, IsActive = c.IsActive, OrderIndex = c.OrderIndex });
		//    return new { Data = lst };

		//}
		[HttpGet]
		public dynamic GetItemsForCategory(int Categoryid, int customerId, int country)
		{
			var lst = _context.Items
				.Include(s => s.Shop)
				.ToList()
				.Where(c => (c.CategoryId == Categoryid) && (c.CountryId == country) && IsShopHasValidSub(c.ShopId.Value))
				.OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					CategoryId = c.CategoryId,
					SubCategoryId = c.SubCategoryId,
					ItemId = c.ItemId,
					ItemName = c.ItemName,
					ItemNameAr = c.ItemNameAr,
					OutOfStock = c.OutOfStock,
					ItemDescriptionAr = c.ItemDescriptionAr,
					ItemImage = "/images/" + c.ItemImage,
					ItemDescription = c.ItemDescription,
					ItemPrice = c.ItemPrice,
					OrderIndex = c.OrderIndex,
					Shop = c.Shop,
					IsFavourite = _context.WishList.FirstOrDefault(w => w.ItemId == c.ItemId && w.CustomerId == customerId) == null ? false : true
				});
			return new { Data = lst };

		}
		[HttpGet]
		public dynamic Pagecontent(string name)
		{

			var lst = _context.PageContent.Where(s => s.PageTitle == name)
				.Select(c => new { Id = c.Id, PageTitle = c.PageTitle, Content = c.Content });
			return new { Data = lst };

		}

		[HttpGet]
		public dynamic GetAboutPage()
		{

			var lst = _context.PageContent.Where(s => s.Id == 1)
				.Select(c => new { Id = c.Id, PageTitle = c.PageTitle, Content = c.Content });
			return new { Data = lst };

		}
		[HttpGet]
		public dynamic GetTermsAndConditionPage()
		{

			var lst = _context.PageContent.Where(s => s.Id == 2)
				.Select(c => new { Id = c.Id, PageTitle = c.PageTitle, Content = c.Content });
			return new { Data = lst };

		}

		[HttpGet]
		public dynamic GetPrivacyAndPolicyPage()
		{

			var lst = _context.PageContent.Where(s => s.Id == 3)
				.Select(c => new { Id = c.Id, PageTitle = c.PageTitle, Content = c.Content });
			return new { Data = lst };

		}


		[HttpGet]
		public IActionResult GetSocialmedia()
		{
			try
			{
				var SocialList=_context.SoicialMidiaLink.Select(c => new {
					id = c.id, 
					WhatsApp = c.WhatsApplink,
					Instgram = c.Instgramlink,
					Twitter = c.TwitterLink 
				});
				return Ok(new { Status = true, SocialList = SocialList });

			}
			catch (Exception e)
			{

				return Ok(new { Status = false, message = e.Message });

			}
		   

		}
		[HttpGet]

		public dynamic ContactUs()
		{

			var lst = _context.ContactUs.Select(c => new ContactUS { id = c.id, phonenumber1 = c.phonenumber1, phonenumber2 = c.phonenumber2, Email = c.Email, Address = c.Address });
			return new { Data = lst };

		}


		[HttpGet]
		public ActionResult<IEnumerable<SubCategory>> GetSubcatgories()
		{
			return _context.SubCategory.Where(s => s.IsActive == true).OrderBy(s => s.OrderIndex).ToList();

		}
		//[HttpPost]
		//[ValidateAntiForgeryToken]
		//public async Task<IActionResult> ForgotPassword(ForgotPasswordModel forgotPasswordModel)
		//{
		//    if (!ModelState.IsValid)
		//        return View(forgotPasswordModel);

		//    var user = await _userManager.FindByEmailAsync(forgotPasswordModel.User);
		//    if (user == null)
		//        return RedirectToAction(nameof(ForgotPasswordConfirmation));

		//    var token = await _userManager.GeneratePasswordResetTokenAsync(user);
		//    var callback = Url.Action(nameof(ResetPassword), "Account", new { token, email = user.Email }, Request.Scheme);

		//    var message = new Message(new string[] { user.Email }, "Reset password token", callback, null);
		//    await _emailSender.SendEmailAsync(message);

		//    return RedirectToAction(nameof(ForgotPasswordConfirmation));
		//}
		[HttpGet]
		public ActionResult<IEnumerable<Category>> GetCatgories()
		{
			return _context.Category.Where(s => s.IsActive == true).OrderBy(s => s.OrderIndex).ToList();
		}

		#region "Page8"
		//Page 8: Searchbar
		[HttpGet]
		[Route("{name}")]
		public dynamic IteamSearch(string name, int country, int customerId)
		{
			var lst = _context.Items.Include(e => e.Shop).Where(i => i.CountryId == country && (i.ItemName.Contains(name) || i.ItemNameAr.Contains(name) || i.ItemDescription.Contains(name) || i.ItemDescriptionAr.Contains(name) || i.Shop.ShopTlar.Contains(name) || i.Shop.ShopTlen.Contains(name))).Select(c => new
			{
				ItemId = c.ItemId,
				CategoryId = c.CategoryId,
				SubCategoryId = c.SubCategoryId,
				ShopId = c.ShopId,
				ItemName = c.ItemName,
				ItemNameAr = c.ItemNameAr,
				ItemDescriptionAr = c.ItemDescriptionAr,
				ItemImage = c.ItemImage,
				ItemDescription = c.ItemDescription,
				ItemPrice = c.ItemPrice,
				OrderIndex = c.OrderIndex,
				Shop = c.Shop,
				OutOfStock = c.OutOfStock,
				IsFavourite = _context.WishList.FirstOrDefault(w => w.ItemId == c.ItemId && w.CustomerId == customerId) == null ? false : true,
				productimage = c.ProductImages.ToList()

			}).ToList();
			//;
			//var lst = _context.Items
			//  .Where(c => c.ItemId == ItemId && c.IsActive == true)
			//  .Include(c => c.ProductImages).OrderBy(c => c.OrderIndex)
			//  .Select(c => new
			//  {
			//      ItemId = c.ItemId,
			//      CategoryId = c.CategoryId,
			//      SubCategoryId = c.SubCategoryId,
			//      ShopId = c.ShopId,
			//      ItemName = c.ItemName,
			//      ItemNameAr = c.ItemNameAr,
			//      ItemDescriptionAr = c.ItemDescriptionAr,
			//      ItemImage = c.ItemImage,
			//      ItemDescription = c.ItemDescription,
			//      ItemPrice = c.ItemPrice,
			//      OrderIndex = c.OrderIndex,
			//      Shop = c.Shop,
			//      OutOfStock = c.OutOfStock,
			//      IsFavourite = _context.WishList.FirstOrDefault(w => w.ItemId == ItemId && w.CustomerId == customerId) == null ? false : true,
			//      productimage = c.ProductImages.ToList()

			//  }).ToList();

			return new { Data = lst };
		}

		#endregion
		#region "Page9"
		[HttpGet]
		//Page 9: SliderImages for Item
		public dynamic ItemImages(int ItemId)
		{

			var lst = _context.ProductImages.Where(c => c.ItemId == ItemId).Select(c => new { ImageId = c.ImageId, ImageName = c.ImageName, ItemId = c.ItemId });
			return new { Data = lst };

		}
		#endregion


		#region "Page12"
		//Page 12: ShoppingCart
		[HttpGet]
		public dynamic GetShoppingCartByCustomerId(int id)
		{

			var shoppingCartList = _context.ShoppingCart
												.Include(s => s.Item)
												.ThenInclude(s => s.Shop)
												.Where(c => c.CustomerId == id)
												.Select(c => new
												{
													ShoppingCartId = c.ShoppingCartId,
													ItemId = c.Item.ItemId,
													ItemName = c.Item.ItemName,
													ItemPhoto = "/images/" + c.Item.ItemImage,
													ItemDescription = c.Item.ItemDescription,
													Itemprice = c.Item.ItemPrice,
													ItemQty = c.ItemQty,
													ShopTlen = c.Item.Shop.ShopTlen,
													ShopId = c.Item.Shop.ShopId
												}).ToList();

			float totalDeliveryCost = 0;

			var tdc = shoppingCartList.GroupBy(c => c.ShopId).

			Select(g => new
			{
				TotalDeliveryCost = _context.Shop.SingleOrDefault(s => s.ShopId == g.Key).Deliverycost

			}).ToList();

			foreach (var item in tdc)
			{
				totalDeliveryCost += item.TotalDeliveryCost.Value;
			}

			return new { Data = shoppingCartList, TotalDeliveryCost = totalDeliveryCost };

		}
		[HttpDelete]
		public dynamic DeleteItemFromShoppingcart(int Itemid, int customerId)
		{

			var Item = _context.ShoppingCart.Where(c => c.ItemId == Itemid && c.CustomerId == customerId).FirstOrDefault();
			_context.ShoppingCart.Remove(Item);
			_context.SaveChanges();
			return (new { Success = true, message = "Item Has Been Deleted From Shoppingcart " });

		}
		[HttpPost]
		public dynamic ADDItemFromShoppingcart([FromBody] ShoppingCart Item)
		{

			var customerId = Item.CustomerId;
			var itemId = Item.ItemId;

			var itemIsExist = _context.ShoppingCart.FirstOrDefault(s => s.ItemId == itemId && s.CustomerId == customerId);

			if (itemIsExist == null)
			{
				_context.ShoppingCart.Add(Item);
				_context.SaveChanges();
				return (new { Success = true, message = "Item Succesfuly Added" });
			}

			itemIsExist.ItemQty = itemIsExist.ItemQty + Item.ItemQty;
			_context.SaveChanges();
			return (new { Success = true, message = "Item Succesfuly Added" });

		}
		[HttpPost]
		public IActionResult ADDListItemsToShoppingcart([FromBody] List<ShoppingCartVM> Items)
		{
			try
			{
				for (int i = 0; i < Items.Count; i++)
				{

					var customerObj = _context.Customer.FirstOrDefault(c => c.CustomerId == Items[i].CustomerId);
					if (customerObj == null)
					{
						return Ok(new { status = false, message = $"Customer Object No {i}  Not Found" });

					}
					var itemObj = _context.Items.FirstOrDefault(c => c.ItemId == Items[i].ItemId);
					if (itemObj == null)
					{
						return Ok(new { status = false, message = $"Item  Object No {i} Not Found" });

					}
					var itemAlreadyExistInCustomerCart =
						_context.ShoppingCart.FirstOrDefault(s =>
						s.ItemId == Items[i].ItemId &&
						s.CustomerId == Items[i].CustomerId);
					var itemId = Items[i].ItemId;
					var shopDeliveryCost = _context.Items.Include(i => i.Shop).FirstOrDefault(i => i.ItemId == itemId).Shop.Deliverycost;

					if (shopDeliveryCost == null)
					{
						shopDeliveryCost = 0;
					}

					if (itemAlreadyExistInCustomerCart != null)
					{
						itemAlreadyExistInCustomerCart.ItemQty += Items[i].ItemQty;
						itemAlreadyExistInCustomerCart.ItemTotal += Items[i].ItemTotal;
						itemAlreadyExistInCustomerCart.ItemPrice = Items[i].ItemPrice;
						itemAlreadyExistInCustomerCart.Deliverycost = shopDeliveryCost.Value;
						_context.Attach(itemAlreadyExistInCustomerCart).State = EntityState.Modified;
						_context.SaveChanges();

					}
					else
					{
						ShoppingCart shoppingItemObj = new ShoppingCart()
						{
							CustomerId = Items[i].CustomerId,
							ItemId = Items[i].ItemId,
							ItemPrice = Items[i].ItemPrice,
							ItemQty = Items[i].ItemQty,
							ItemTotal = Items[i].ItemTotal,
							Deliverycost = shopDeliveryCost.Value
						};

						_context.ShoppingCart.Add(shoppingItemObj);
						_context.SaveChanges();
					}
				}
				return Ok(new { status = true, message = "List Added Successfully" });

			}
			catch (Exception ex)
			{
				return Ok(new { status = false, message = ex.Message });

			}
		}

		[HttpPost]
		public dynamic ADDItemTowishList([FromBody] WishList item)
		{

			_context.WishList.Add(item);
			_context.SaveChanges();
			return (new { Success = true, message = "Item Succesfuly Added To Your Favourite Items " });

		}

		[HttpPost]
		public ActionResult<object> Makeorder([FromBody] OrderModel orderModel)
		{

			var shoppingCartList = _context.ShoppingCart.Where(s => s.CustomerId == orderModel.CustomerId).ToList();

			if (shoppingCartList.Count() > 0)
			{

				Order order = new Order();
				order.CustomerId = orderModel.CustomerId;
				order.IsDeliverd = false;
				order.ShippingAddressId = orderModel.AdressId;
				order.PaymentMehodPaymentMethod = _context.PaymentMehod.FirstOrDefault(p => p.PaymentMethodName == orderModel.PaymentMethodName);

				_context.Order.Add(order);
				var votcer = _context.Coupon.FirstOrDefault(s => s.Serial == orderModel.CouponSerial);

				if (votcer != null && votcer.Used == false)
				{
					_context.SaveChanges();
				}
				else
				{
					return "Votcher not correct";
				}

				foreach (var item in shoppingCartList)
				{
					ItemsOrderTable2 it = new ItemsOrderTable2()
					{
						ItemId = (int)item.ItemId,
						Quantity = (int)item.ItemQty,
						OrderId = order.OrderId,

					};

					_context.ItemsOrderTable2.Add(it);
					_context.ShoppingCart.Remove(item);
					_context.SaveChanges();
				}

				//var itemsordertabelinfo = shoppingCartList.GroupBy(s => s.ItemId).Select(s => new ItemsOrderTable
				//{
				//    Quantity = s.Count(),
				//    OrderId = order.OrderId,
				//    ItemId = s.Key.Value

				//});

				//foreach (var item in itemsordertabelinfo)
				//{
				//    _context.ItemsOrder.Add(item);
				//    _context.SaveChanges();
				//}



				return (new { Success = true, message = "Your Order has Been Added " });

			}

			return (new { Success = false, message = "Shopping Cart Is Empty" });

		}

		[HttpDelete]
		public dynamic DeleteItemFromWishlist(int Itemid, int customerId)
		{

			var Item = _context.WishList.Where(c => c.ItemId == Itemid && c.CustomerId == customerId).FirstOrDefault();
			_context.WishList.Remove(Item);
			_context.SaveChanges();
			return (new { Success = true, message = "Item Has Been Deleted from Favourite Items" });

		}
		[HttpPost]
		public dynamic ChangeItemQty(int Itemid, int customerId, int Qty)
		{

			var item = _context.ShoppingCart.FirstOrDefault(c => c.ItemId == Itemid && c.CustomerId == customerId);

			item.ItemQty = Qty;
			item.ItemTotal = Qty * item.ItemPrice;
			_context.SaveChanges();
			return (new { Success = true, message = "Item Qty Increased" });

		}
		[HttpPost]
		public dynamic GetFavuroiteItemByCustomerId(int Customerid)
		{

			var lst = _context.WishList.Where(c => c.CustomerId == Customerid).Include(c => c.Item).ThenInclude(c => c.Shop).Select(c => new
			{
				WishlistId = c.WishlistId,
				CustomerId = c.CustomerId,
				temId = c.ItemId,
				ItemName = c.Item.ItemName,
				ItemDescription = c.Item.ItemDescription,
				ItemPic = "/images/" + c.Item.ItemImage,
				ItemPrice = c.Item.ItemPrice,
				ShopId = c.Item.ShopId,
				ShopTlen = c.Item.Shop.ShopTlen,
				ShopTlar = c.Shop.ShopTlar,
				CategoryId = c.Item.CategoryId
			});
			return new { Data = lst };

		}
		#endregion
		#region "Page13"
		//page 13: login
		[HttpGet]
		public async Task<ActionResult<IdentityUser>> Login([FromQuery] string Email, [FromQuery] string Password)
		{

			var user = await _userManager.FindByEmailAsync(Email);

			if (user != null)
			{

				var result = await _signInManager.CheckPasswordSignInAsync(user, Password, true);

				if (result.Succeeded)
				{

					var Customer = _context.Customer.Where(r => r.CustomerEmail == user.Email).Select(i => new Customer
					{
						CustomerId = i.CustomerId,
						CustomernameAr = i.CustomernameAr,
						CustomernameEn = i.CustomernameEn,
						CustomerAddress = i.CustomerAddress,
						Customerphone = i.Customerphone,
						CustomerEmail = i.CustomerEmail,

					});


					var validResponse = new { status = true, customerInfo = Customer };
					return Ok(validResponse);
				}


			}

			var invalidResponse = new { status = false };

			return Ok(invalidResponse);

		}
		[HttpPost]

		public async Task<IActionResult> Register([FromQuery] string email, [FromQuery] string password, [FromQuery] string mobile, [FromQuery] string EnglihName, [FromQuery] string ArabicName)
		{
			var userExists = await _userManager.FindByEmailAsync(email);

			if (userExists != null)
				return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User already exists!" });

			var user = new ApplicationUser { UserName = email, Email = email };
			var result = await _userManager.CreateAsync(user, password);

			if (!result.Succeeded)
				return StatusCode(StatusCodes.Status500InternalServerError, new { Status = "Error", Message = "User creation failed! Please check user details and try again." });
			DefaultAvatar def = new DefaultAvatar(_hostEnvironment);
			var uniqeFileName = def.CreateProfilePicture(EnglihName);

			Customer customer = new Customer()
			{
				CustomerEmail = email,
				CustomernameEn = EnglihName,
				CustomernameAr = ArabicName,
				Customerphone = mobile,
				RegisterDate = DateTime.Now,
				CustomerImage = uniqeFileName

			};

			_context.Customer.Add(customer);
			_context.SaveChanges();




			user.EntityId = customer.CustomerId;
			user.Pic = uniqeFileName;
			_db.SaveChanges();

			return Ok(new { Status = "Success", customerInfo = customer, Message = "User created successfully!" });
		}

		#endregion
		#region "Page14"
		//Page 14: Checkout PayingMethod
		[HttpGet]
		public dynamic GetPayingMethod()
		{

			var lst = _context.PaymentMehod.Where(e=>e.IsActive==true)
				.Select(c => new
				{
					PaymentMethodId = c.PaymentMethodId,
					PaymentMethodName = c.PaymentMethodName,
					PaymentPaymentMethodPic = "/Images/PaymentMethod/" + c.PaymentMethodPic
				});
			return new { Data = lst };

		}
		#endregion
		#region "Page16 to Page21"
		//Page 20: Address
		[HttpGet]
		public dynamic GetAddressByCustomerId(int CustomerId)
		{

			var lst = _context.ShippingAddress.Where(c => c.CustomerId == CustomerId).Select(c => new { ShippingAddressId = c.ShippingAddressId, CustomerId = c.CustomerId, AddressNickname = c.AddressNickname, Area = c.Area, AddressTypeId = c.AddressTypeId, Street = c.Street, Block = c.Block, Avenue = c.Avenue, Building = c.Building, Floor = c.Floor, Office = c.Office, AdditionalDirection = c.AdditionalDirection, AddressType = c.AddressType });
			return new { Data = lst };

		}
		[HttpPost]
		public IActionResult shippingAddress([FromBody] ShopingAddressVM addressVM)
		{
			var customer = _context.Customer.Where(e => e.CustomerId == addressVM.CustomerId).FirstOrDefault();
			if (customer == null)
			{
				return Ok(new { status = false, Message = "Please! Enter Valid Customer Id" });
			}
			var addressType = _context.AddressType.Where(e => e.AddressTypeId == addressVM.AddressTypeId).FirstOrDefault();

			if (addressType == null)
			{
				return Ok(new { status = false, Message = "Please! Enter Valid AddressTypeID Id" });
			}
			var shopingAddressObj = new ShippingAddress()
			{
				AddressTypeId = addressVM.AddressTypeId,
				AddressNickname = addressVM.AddressNickname,
				Area = addressVM.Area,
				Block = addressVM.Block,
				Avenue = addressVM.Avenue,
				Building = addressVM.Building,
				Floor = addressVM.Floor,
				Office = addressVM.Office,
				Lat = addressVM.Lat,
				Lng = addressVM.Lng,
				Street = addressVM.Street,
				AdditionalDirection = addressVM.AdditionalDirection,
				CustomerId = addressVM.CustomerId
			};
			try
			{
				_context.ShippingAddress.Add(shopingAddressObj);
				_context.SaveChanges();
				return Ok(new { Success = true, ShoppingAddress = shopingAddressObj });
			}
			catch (Exception e)
			{
				return Ok(new { status = false, Reason = e.Message });


			}
		}
		[HttpGet]
		public async Task<IActionResult> GetAddressTypeList()
		{
			try
			{
				//for (int i = 1; i < 3; i++)
				//{
				//    var ins = new AddressType()
				//    {
				//        AddressType1 = $"AddressType1 {i}"
				//    };
				//    _context.AddressType.Add(ins);
				//    _context.SaveChanges();

				//}

				var List = await _context.AddressType.ToListAsync();


				return Ok(new { status = true, List });

			}
			catch (Exception)
			{

				return Ok(new { status = false, message = "Something went wrong" });

			}

		}

		[HttpDelete]
		public dynamic DeleteAddress(int AddressId)
		{

			var Address = _context.ShippingAddress.FirstOrDefault(c => c.ShippingAddressId == AddressId);

			if (_context.Order.Where(o => o.ShippingAddressId == AddressId).Count() > 0)
			{
				return (new { Success = false, message = "You cannot delete this address because it has been used before" });
			}
			_context.ShippingAddress.Remove(Address);
			_context.SaveChanges();

			return (new { Success = true, message = "Address Has Been Deleted  " });

		}

		[HttpPut]
		public ShippingAddress EditAddress([FromBody] ShippingAddress ChangedAddress)
		{

			ShippingAddress address = _context.ShippingAddress.AsNoTracking().FirstOrDefault(e => e.ShippingAddressId == ChangedAddress.ShippingAddressId);

			if (address != null)
			{
				_context.Entry(ChangedAddress).State = EntityState.Modified;
				_context.SaveChanges();

			}
			return ChangedAddress;

		}

		#endregion
		//public List<ProductImage> ItemImages(int ItemId )
		//{

		//   return _context.ProductImage.Where(i=>i.ItemId== ItemId)
		//}

		[HttpGet]
		public dynamic GetCategoryList()
		{

			var lst = _context.Category.OrderBy(c => c.OrderIndex)
				.Select(c => new { c.CategoryId, c.CategoryTlar, c.CategoryTlen, CategoryPic = "/images/categories/" + c.CategoryPic, c.OrderIndex, CategoryIcon = "/images/categories/" + c.CategoryIcon });
			return new { Data = lst };
		}
		[HttpGet]
		public dynamic GetSubCategoryForEachcategory()
		{

			var lst = _context.Category.OrderBy(c => c.OrderIndex).Select(c => new { c.SubCategory });
			return new { Data = lst };
		}
		// Customer
		[HttpGet]
		public dynamic GetCustomerList()
		{

			var lst = _context.Customer.Select(c => new Customer { CustomerId = c.CustomerId, CustomernameAr = c.CustomernameAr, CustomernameEn = c.CustomernameEn, CustomerAddress = c.CustomerAddress, Customerphone = c.Customerphone, CustomerEmail = c.CustomerEmail, ShippingAddress = c.ShippingAddress.ToList(), ShoppingCart = c.ShoppingCart.ToList() });
			return new { Data = lst };

		}
		//protected void SendEmail(object sender, EventArgs e)
		//{
		//    string username = string.Empty;
		//    string password = string.Empty;
		//    string constr = ConfigurationManager.ConnectionStrings["constr"].ConnectionString;
		//    using (SqlConnection con = new SqlConnection(constr))
		//    {
		//        using (SqlCommand cmd = new SqlCommand("SELECT Username, [Password] FROM Users WHERE Email = @Email"))
		//        {
		//            cmd.Parameters.AddWithValue("@Email", txtEmail.Text.Trim());
		//            cmd.Connection = con;
		//            con.Open();
		//            using (SqlDataReader sdr = cmd.ExecuteReader())
		//            {
		//                if (sdr.Read())
		//                {
		//                    username = sdr["Username"].ToString();
		//                    password = sdr["Password"].ToString();
		//                }
		//            }
		//            con.Close();
		//        }
		//    }
		//    if (!string.IsNullOrEmpty(password))
		//    {
		//        MailMessage mm = new MailMessage("sender@gmail.com", txtEmail.Text.Trim());
		//        mm.Subject = "Password Recovery";
		//        mm.Body = string.Format("Hi {0},<br /><br />Your password is {1}.<br /><br />Thank You.", username, password);
		//        mm.IsBodyHtml = true;
		//        SmtpClient smtp = new SmtpClient();
		//        smtp.Host = "smtp.gmail.com";
		//        smtp.EnableSsl = true;
		//        NetworkCredential NetworkCred = new NetworkCredential();
		//        NetworkCred.UserName = "sender@gmail.com";
		//        NetworkCred.Password = "<Password>";
		//        smtp.UseDefaultCredentials = true;
		//        smtp.Credentials = NetworkCred;
		//        smtp.Port = 587;
		//        smtp.Send(mm);
		//        lblMessage.ForeColor = Color.Green;
		//        lblMessage.Text = "Password has been sent to your email address.";
		//    }
		//    else
		//    {
		//        lblMessage.ForeColor = Color.Red;
		//        lblMessage.Text = "This email address does not match our records.";
		//    }
		//}
		[HttpDelete]
		public dynamic DeleteCustomer(int id)
		{

			var customer = _context.Customer.Where(c => c.CustomerId == id).FirstOrDefault();
			_context.Customer.Remove(customer);
			_context.SaveChanges();
			return customer;

		}
		[HttpPost]
		public Customer ADDCustomer([FromBody] Customer customer)
		{
			_context.Customer.Add(customer);
			_context.SaveChanges();
			return customer;
		}
		[HttpPost]
		public ActionResult<object> ADDCustomerPic([FromBody] CustomerPic customerPicModel)
		{
			var customer = _context.Customer.FirstOrDefault(c => c.CustomerId == customerPicModel.CustomerId);

			if (customer == null)
			{
				return (new { Success = false, message = "Customer Not Found" });
			}


			var bytes = Convert.FromBase64String(customerPicModel.CustomerPicture);

			string pathOfAllPictures = Path.Combine(_hostEnvironment.WebRootPath, "images");

			string uniqePictureName = Guid.NewGuid() + "_" + customerPicModel.CustomerId + ".jpeg";
			string uploadedImagePath = Path.Combine(pathOfAllPictures, uniqePictureName);

			using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
			{
				imageFile.Write(bytes, 0, bytes.Length);
				imageFile.Flush();
			}

			var picPath = "/images/" + uniqePictureName;

			customer.CustomerImage = uniqePictureName;
			_context.SaveChanges();

			_db.ApplicationUsers.FirstOrDefault(c => c.EntityId == customerPicModel.CustomerId).Pic = uniqePictureName;
			_db.SaveChanges();

			return (new { Success = true, message = "Pic has been Sucessfull", picPath = picPath });
		}


		[HttpPost]
		public ActionResult<object> ResetPassword([FromBody] ResetPasswordViewModel resetPasswordViewModel)
		{

			var customer = _db.ApplicationUsers.FirstOrDefault(c => c.EntityId == resetPasswordViewModel.CustomerId);

			var result = _userManager.ChangePasswordAsync(customer, resetPasswordViewModel.OldPassword, resetPasswordViewModel.NewPassword);

			if (result.Result.Succeeded)
			{
				return new { message = "password has changed successfuly" };

			}
			else
			{
				return new { message = "Error" };

			}


		}


		[HttpPut]
		public Customer EditCustomer([FromBody] Customer ChangedCustomer)
		{


			Customer customer = _context.Customer.FirstOrDefault(e => e.CustomerId == ChangedCustomer.CustomerId);

			if (customer != null)
			{
				if (System.IO.File.Exists(customer.CustomerImage))
				{
					System.IO.File.Delete(customer.CustomerImage);
				}

				if (ChangedCustomer.CustomerImage != null)
				{
					var bytes = Convert.FromBase64String(ChangedCustomer.CustomerImage);

					string pathOfAllPictures = Path.Combine(_hostEnvironment.WebRootPath, "images");

					string uniqePictureName = Guid.NewGuid() + "_" + customer.CustomerId + ".jpeg";
					string uploadedImagePath = Path.Combine(pathOfAllPictures, uniqePictureName);

					using (var imageFile = new FileStream(uploadedImagePath, FileMode.Create))
					{
						imageFile.Write(bytes, 0, bytes.Length);
						imageFile.Flush();
					}
					var picPath = "/images/" + uniqePictureName;
					customer.CustomerImage = ChangedCustomer.CustomerImage;
				}




				customer.CustomernameAr = ChangedCustomer.CustomernameAr;
				customer.CustomernameEn = ChangedCustomer.CustomernameEn;
				customer.CustomerEmail = ChangedCustomer.CustomerEmail;
				customer.Customerphone = ChangedCustomer.Customerphone;
				customer.CustomerAddress = ChangedCustomer.CustomerAddress;

				_db.ApplicationUsers.FirstOrDefault(u => u.EntityId == ChangedCustomer.CustomerId).Email = ChangedCustomer.CustomerEmail;
				_db.ApplicationUsers.FirstOrDefault(u => u.EntityId == ChangedCustomer.CustomerId).UserName = ChangedCustomer.CustomerEmail;
				_db.SaveChanges();



			}
			_context.SaveChanges();

			return customer;

		}
		[HttpGet]
		public dynamic GetCustomerByCustomerId(int CustomerId)
		{

			var customer = _context.Customer
				.Where(c => c.CustomerId == CustomerId)
				.Select(c => new
				{
					CustomerId = c.CustomerId,
					CustomernameAr = c.CustomernameAr,
					CustomernameEn = c.CustomernameEn,
					CustomerAddress = c.CustomerAddress,
					Customerphone = c.Customerphone,
					CustomerEmail = c.CustomerEmail,
					CustomerImage = "/images/" + c.CustomerImage,
				});

			return new { Data = customer };
		}
		//public Customer GetCustomer(int id)
		//{

		//    var Customer = _context.Customer.Where(c=>c.CustomerId==id).FirstOrDefault();
		//    return Customer;

		//}

		//Order
		[HttpGet]
		public dynamic GetOrderList()
		{

			var lst = _context.Order.OrderBy(c => c.OrderIndex)
				.Select(c => new Order
				{
					OrderId = c.OrderId,
					CustomerId = c.CustomerId,
					ShippingAddressId = c.ShippingAddressId,
					IsDeliverd = c.IsDeliverd,
					Notes = c.Notes,
					OrderIndex = c.OrderIndex,
					Customer = c.Customer
				});
			return new { Data = lst };

		}
		[HttpGet]
		public dynamic GetOrderByCustomerId(int CustomerId)
		{

			var lst = _context.Order.Where(c => c.CustomerId == CustomerId)
				.Include(s => s.PaymentMehodPaymentMethod)
				.Select(c => new
				{
					OrderId = c.OrderId,

					CustomerId = c.CustomerId,
					ShippingAddressId = c.ShippingAddressId.Value,
					OrderTotal = c.OrderTotal,
					Deliverycost = c.Deliverycost,
					IsDeliverd = c.IsDeliverd,
					PaymentMehodPaymentMethod = c.PaymentMehodPaymentMethod,
					ShippingAddress = _context.ShippingAddress.FirstOrDefault(s => s.ShippingAddressId == c.ShippingAddressId),
					OrderDiscount = c.OrderDiscount,
					OrderDate = c.OrderDate,
					items = _context.OrderItems
					.Include(w => w.Item).Include(c => c.Item.Shop)
					.Where(o => o.OrderId == c.OrderId)
					.Select(s => new
					{
						ItemDetails = s.Item,
						ItemPrice = s.ItemPrice,
						ItemQuantity = s.ItemQuantity,
						shop = s.Item.Shop
					}).ToList()

				});


			return new { Data = lst };

		}

		[HttpGet]
		public dynamic GetOrderByOrderId(int OrderId)
		{

			var lst = _context.
				Order.Where(c =>
				c.OrderId == OrderId)
				.Include(c => c.ItemsOrderTable2)
				.Select(c => new Order
				{
					OrderId = c.OrderId,
					CustomerId = c.CustomerId,
					ShippingAddressId = c.ShippingAddressId,
					IsDeliverd = c.IsDeliverd,
					Notes = c.Notes,
					OrderIndex = c.OrderIndex,
					PaymentMehodPaymentMethod = c.PaymentMehodPaymentMethod,
					OrderDiscount = c.OrderDiscount,
					Deliverycost = c.Deliverycost,
					GetOrderModel = c.ItemsOrderTable2.ToList().Select(s => new OrderItemsList { ItemName = _context.Items.FirstOrDefault(a => a.ItemId == s.ItemId).ItemName, ItemDescription = _context.Items.FirstOrDefault(a => a.ItemId == s.ItemId).ItemDescription, ItemPrice = _context.Items.FirstOrDefault(a => a.ItemId == s.ItemId).ItemPrice })
				});

			return new { Data = lst };

		}
		[HttpPost]
		public Order AddOrder([FromBody] Order order)
		{
			_context.Order.Add(order);
			_context.SaveChanges();
			return order;
		}
		[HttpPut]
		public Order EditOrder(Order ChangedOrder)
		{

			Order order = _context.Order.FirstOrDefault(e => e.OrderId == ChangedOrder.OrderId);
			if (order != null)
			{


				//order.Item = ChangedOrder.Item;
				//order.Quantity = ChangedOrder.Quantity;
				order.Notes = ChangedOrder.Notes;
				order.IsDeliverd = ChangedOrder.IsDeliverd;

			}
			_context.SaveChanges();
			return order;

		}
		[HttpDelete]
		public dynamic DeleteOrder(int id)
		{

			var Order = _context.Order.Where(c => c.OrderId == id).FirstOrDefault();
			_context.Order.Remove(Order);
			_context.SaveChanges();
			return Order;

		}
		// WishList
		[HttpGet]
		public dynamic GetFavouriteItemByCustomerId(int CustomerId)
		{

			var customer = _context.WishList.Where(c => c.CustomerId == CustomerId).Include(s => s.Item).Select(c => new
			{
				WishlistId = c.WishlistId,
				CustomerId = c.CustomerId,
				ItemId = c.ItemId,
				Item = c.Item
			});

			return new { Data = customer };
		}

		//BestOffer
		[HttpGet]
		public dynamic getbestoffers()
		{

			var lst = _context.BestOffer
				.Select(c => new BestOffer
				{
					BestOfferId = c.BestOfferId,
					Subcategoryid = c.Item.SubCategoryId,
					Categoryid = c.Item.CategoryId,
					ItemId = c.Item.ItemId,
					ItemName = c.Item.ItemName,
					ItemPhoto = "/images/" + c.Item.ItemImage,
					ItemDescription = c.Item.ItemDescription,
					ItemPrice = c.ItemPrice,
					Discount = c.Discount,
					Shop = c.Shop
				}
					);
			return new { data = lst };

		}
		//Item
		[HttpGet]
		public object GetItems(int country)
		{

			var lst = _context.Items
				.Include(s => s.Shop)
				.ToList()
				.Where(i => (i.IsActive == true) && (i.CountryId == country) && IsShopHasValidSub(i.ShopId.Value))
				.OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					ItemId = c.ItemId,
					CategoryId = c.CategoryId,
					SubCategoryId = c.SubCategoryId,
					ShopId = c.ShopId,
					ItemName = c.ItemName,
					ItemImage = "/images/" + c.ItemImage,
					ItemDescription = c.ItemDescription,
					ItemPrice = c.ItemPrice,
					OrderIndex = c.OrderIndex,
					Category = c.Category,
					Shop = c.Shop,
					SubCategory = c.SubCategory,
					ItemNameAr = c.ItemNameAr,
					ItemDescriptionAr = c.ItemDescriptionAr,
				});
			return new { Data = lst };

		}
		[HttpGet]
		public dynamic GetItemById(int ItemId, int customerId)
		{

			var lst = _context.Items
				.Where(c => c.ItemId == ItemId && c.IsActive == true)
				.Include(c => c.ProductImages).OrderBy(c => c.OrderIndex)
				.Select(c => new
				{
					ItemId = c.ItemId,
					CategoryId = c.CategoryId,
					SubCategoryId = c.SubCategoryId,
					ShopId = c.ShopId,
					ItemName = c.ItemName,
					ItemNameAr = c.ItemNameAr,
					ItemDescriptionAr = c.ItemDescriptionAr,
					ItemImage = c.ItemImage,
					ItemDescription = c.ItemDescription,
					ItemPrice = c.ItemPrice,
					OrderIndex = c.OrderIndex,
					Shop = c.Shop,
					OutOfStock = c.OutOfStock,
					IsFavourite = _context.WishList.FirstOrDefault(w => w.ItemId == ItemId && w.CustomerId == customerId) == null ? false : true,
					productimage = c.ProductImages.ToList()

				}).ToList();
			return new { Data = lst };

		}
		//ChampionShip
		[HttpGet]
		public dynamic GetChampionShip()
		{

			var lst = _context.ChampionShip
				.Select(c => new { ChampionshipId = c.ChampionshipId, ChampionshipName = c.ChampionshipName, ChampionshipPhoto = "/images/" + c.ChampionshipPhoto });
			return new { Data = lst };

		}
		[HttpGet]
		public ActionResult<object> GetChampionBanner()
		{

			var banners = _context.ChampionBanners.ToList();

			return new { Data = banners };

		}

		[HttpGet]
		[Route("{name}")]
		public dynamic ChampionshipSearch(string ChampionshipName)
		{
			var lst = _context.ChampionShip.Where(i => i.ChampionshipName.Contains(ChampionshipName));
			return new { Data = lst };
		}

		[HttpGet]
		[Route("{id}")]
		public ActionResult<object> GetChampionById(int id)
		{

			var championDetails =
				 _context.Champions.Include(c => c.GameMode).Where(c => c.ChampionId == id)
					.Select(c =>
						new
						{
							ChampionId = c.ChampionId,
							ChampionTlAR = c.ChampionTlAR,
							ChampionTlEn = c.ChampionTlEn,
							StartDate = c.StartDate,
							EndDate = c.EndDate,
							JoinStart = c.JoinStart,
							JoinEnd = c.JoinEnd,
							ChampionPic = "/images/" + c.ChampionPic,
							ChampionContent = c.ChampionContent,
							GameModelId = c.GameModeId,
							GameMode = c.GameMode,
							ChampionParticipates = _context.ChampionParticipates.Include(c => c.Customer).Where(cc => cc.ChampionId == id).ToList(),
						}).ToList();

			return new { Data = championDetails };

		}



		[HttpGet]
		[Route("{id}")]
		public ActionResult<object> GetAllChampion(int id)
		{

			var championDetails =
				 _context.Champions.Include(c => c.GameMode).Where(c => c.ChampTypeId == id)
					.Select(c =>
						new
						{
							ChampionId = c.ChampionId,
							ChampionTlAR = c.ChampionTlAR,
							ChampionTlEn = c.ChampionTlEn,
							StartDate = c.StartDate,
							EndDate = c.EndDate,
							JoinStart = c.JoinStart,
							JoinEnd = c.JoinEnd,
							ChampionPic = "/images/" + c.ChampionPic,
							ChampionContent = c.ChampionContent,
							GameModelId = c.GameModeId,
							GameMode = c.GameMode,
							ChampionParticipates = _context.ChampionParticipates.Include(s => s.Customer).Where(cc => cc.ChampionId == c.ChampionId).ToList(),
						}).ToList();

			return new { Data = championDetails };

		}


		[HttpGet]
		[Route("{customerId}/{champId}")]
		public ActionResult<object> CustomerIsParticipate(int customerId, int champId)
		{

			var customerInChamp =
				_context.ChampionParticipates.FirstOrDefault(c => c.CustomerId == customerId && c.ChampionId == champId);

			if (customerInChamp != null)
			{
				return new { success = true };
			}

			return new { success = false };

		}


		[HttpGet]
		public ActionResult<IEnumerable<Category>> GetTodoItems()
		{
			return _context.Category.ToList();
		}

		// GET api/<ValuesController>/5
		//[HttpGet("{id}")]
		//public string Get(int id)
		//{
		//    return "value";
		//}

		// POST api/<ValuesController>
		[HttpPost]
		public void Post([FromBody] string value)
		{
		}

		// PUT api/<ValuesController>/5
		[HttpPut("{id}")]
		public void Put(int id, [FromBody] string value)
		{
		}

		// DELETE api/<ValuesController>/5
		[HttpDelete("{id}")]
		public void Delete(int id)
		{
		}


		//[HttpGet]
		//public dynamic GetSliderTypesList()
		//{

		//    var lst = _context.SliderType.Select(c => new { c.SliderTypeId, c.SliderTypeTlar, c.SliderTypeTlen, c.Slider });
		//    return new { Data = lst };

		//}



		// Platform 
		[HttpGet]
		public dynamic GetPLatformList()
		{
			var lst = _context.Platforms.OrderBy(c => c.OrderIndex).Select(c => new Platform { PlatformId = c.PlatformId, PlatformTLEN = c.PlatformTLEN, PlatformTLAR = c.PlatformTLAR, PlatformPic = c.PlatformPic, OrderIndex = c.OrderIndex });
			return new { Data = lst };
		}

		[HttpGet]
		public dynamic GetGamesListByPlatformID(int PlatformId)
		{
			var lst = _context.PlatformGames.Where(c => c.PlatFormId == PlatformId).OrderBy(c => c.OrderIndex).Select(c => new PlatformGame { PlatformGameId = c.PlatformGameId, PlatFormId = c.PlatFormId, platform = c.platform, GameTLEN = c.GameTLEN, GameTLAR = c.GameTLAR, OrderIndex = c.OrderIndex });
			return new { Data = lst };
		}

		[HttpPost]
		[Route("{customerID}/{platformID}")]
		public ActionResult<object> SetCustomerFavoutitePlaformAndGames(int customerID, int platformID, [FromBody] int[] gamesIDs)
		{
			var customerFav = _context.CustomerPlatsGamesFavourite.Where(c => c.CustomerId == customerID);

			if (customerFav.Count() > 0)
			{
				foreach (var item in customerFav)
				{
					_context.CustomerPlatsGamesFavourite.Remove(item);
				}

				_context.SaveChanges();
			}

			foreach (var item in gamesIDs)
			{
				var customerPlatsGamesFavourite = new CustomerPlatsGamesFavourite()
				{
					PlatformGameId = item,
					CustomerId = customerID,
					PlatformId = platformID
				};

				_context.CustomerPlatsGamesFavourite.Add(customerPlatsGamesFavourite);
				//_context.Order.Add(order);

			}

			_context.SaveChanges();
			return new { success = true };
		}


		[HttpGet]
		public dynamic GetChampionSliderList()
		{


			var lst = _context.Slider.OrderBy(c => c.OrderIndex).Where(e => e.IsActive == true && e.SliderTypeId == 3).Select(c => new { SliderId = c.SliderId, pic = "/images/" + c.Pic, IsActivity = c.IsActive, SliderTypeId = c.SliderTypeId, SliderType = c.SliderType, EntityId = c.EntityId, OrderIndex = c.OrderIndex });
			return new { Data = lst };

		}


		[HttpGet]
		public dynamic GetChampionGroup()
		{

			var lst = _context.ChampionGroups.Select(c => new ChampionGroup { ChampionGroupID = c.ChampionGroupID, ChampionGroupTlAr = c.ChampionGroupTlAr, ChampionGroupEn = c.ChampionGroupEn });
			return new { Data = lst };
		}

		[HttpGet]
		public dynamic GetChampionGroupList(int GroupId)
		{

			var lst = _context.ChampionGroupLists.Where(c => c.ChampionGroupId == GroupId).Select(c => new ChampionGroupList { ChampionGroupListId = c.ChampionGroupListId, ChampionGroupId = c.ChampionGroupId, ChampionGroup = c.ChampionGroup, ChampionId = c.ChampionId, champion = c.champion });
			return new { Data = lst };
		}

		[HttpGet]
		public dynamic GetPastChampionList()
		{

			//Get Champion List where datetime.now > (startjoin and EndDate and end join and startdate)
			return null;
		}

		[HttpGet]
		public dynamic GetUpcomingChampionList()
		{

			//Get Champion List where datetime.now < (startjoin and EndDate and end join and startdate)
			return null;
		}

		[HttpGet]
		public dynamic GetOngoingChampionList()
		{

			//Get Champion List where datetime.now between (Startjoin Date And EndDate)
			return null;
		}

		[HttpGet]
		[Route("{platformId}")]
		public ActionResult<object> GetPLatformGames(int platformId)
		{
			var gamesPlatformList = _context.PlatformGames.Where(p => p.PlatFormId == platformId).ToList();

			return new { data = gamesPlatformList };
		}

		//public dynamic GetFavouritePlatform()
		//{


		//    var lst = _context.CustomerFavourites.Select(c => new { CustomerFavouriteId = c.CustomerFavouriteId, CustomerId = c.CustomerId, Customer = c.Customer, PlatformId = c.PlatformId, Platform = c.Platform });
		//    return new { Data = lst };
		//}
		//[HttpPost]
		//public dynamic SetFavouritePlatform([FromBody] CustomerFavourite Platform)
		//{

		//    _context.CustomerFavourites.Add(Platform);
		//    _context.SaveChanges();
		//    return (new { Success = true, message = "Platform Succesfuly Added To Your Favourite Platform" });

		//}

		//[HttpPost]
		//[Route("{customerId}/{favPlatformId}")]
		//public dynamic SetCustomerFavouritePlatform(int customerId, int favPlatformId)
		//{

		//    _context.CustomerFavourites.Add(Platform);
		//    _context.SaveChanges();
		//    return (new { Success = true, message = "Platform Succesfuly Added To Your Favourite Platform" });

		//}




		//Ordering

		[HttpGet]
		public Object ValidateCopoun([FromQuery] string CouponSerial)
		{
			//Retrun Success with couponId
			var coupon = _context.Coupon.FirstOrDefault(c => c.Serial == CouponSerial);

			if ((coupon != null) &&
				(DateTime.Now.Date >= coupon.IssueDate.Date) &&
				(DateTime.Now.Date <= coupon.ExpirationDate.Date) &&
				(coupon.Used != true))
			{
				return (new { Success = true, message = "Coupon Exist", CouponId = coupon.Id, CouponTypeId = coupon.CouponTypeId, CouponAmount = coupon.Amount });
			}

			return (new { Success = false, message = "Coupon Not Exist" });

		}




		[HttpPost]
		public Object AddItemToCart([FromBody] AddShoppingCartViewModel addShoppingCartViewModel)
		{
			var itemAlreadyExistInCustomerCart =
				_context.ShoppingCart.FirstOrDefault(s =>
				s.ItemId == addShoppingCartViewModel.ItemId &&
				s.CustomerId == addShoppingCartViewModel.CustomerId);

			var shopDeliveryCost = _context.Items.Include(i => i.Shop).FirstOrDefault(i => i.ItemId == addShoppingCartViewModel.ItemId).Shop.Deliverycost;

			if (shopDeliveryCost == null)
			{
				shopDeliveryCost = 0;
			}

			if (itemAlreadyExistInCustomerCart != null)
			{
				itemAlreadyExistInCustomerCart.ItemQty += addShoppingCartViewModel.ItemQuantity;
				itemAlreadyExistInCustomerCart.ItemTotal += addShoppingCartViewModel.ItemTotal;
				itemAlreadyExistInCustomerCart.ItemPrice = addShoppingCartViewModel.ItemPrice;
				itemAlreadyExistInCustomerCart.Deliverycost = (float)shopDeliveryCost;


				_context.SaveChanges();

				return (new { Success = true, message = "Item Succesfuly Added" });

			}

			ShoppingCart sh = new ShoppingCart()
			{
				CustomerId = addShoppingCartViewModel.CustomerId,
				ItemId = addShoppingCartViewModel.ItemId,
				ItemPrice = addShoppingCartViewModel.ItemPrice,
				ItemQty = addShoppingCartViewModel.ItemQuantity,
				ItemTotal = addShoppingCartViewModel.ItemTotal,
				Deliverycost = (float)shopDeliveryCost
			};

			_context.ShoppingCart.Add(sh);
			_context.SaveChanges();

			return (new { Success = true, message = "Item Succesfuly Added" });
		}

		[HttpPost]
		public object CheckOut([FromBody] CheckOutViewModel checkOutViewModel)
		{

			float discount = 0.0f;

			//Get Customer ShoppingCart Items List
			var customerShoppingCartList = _context.
				ShoppingCart.Include(s => s.Customer)
				.Include(s => s.Item)
				.ThenInclude(s => s.Shop)
				.Where(c => c.CustomerId == checkOutViewModel.CustomerId);

			var totalOfAll = customerShoppingCartList.AsEnumerable().Sum(c => c.ItemTotal);

			// make coupon used
			Coupon coupon = null;

			if (checkOutViewModel.CouponId != null)
			{
				coupon = _context.Coupon.FirstOrDefault(c => c.Id == checkOutViewModel.CouponId);

				if (coupon != null)
				{
					coupon.Used = true;
					_context.SaveChanges();
				}
			}


			//calc ordernet
			float calcOrderNet(float sumItemTotal)
			{
				var percent = sumItemTotal / totalOfAll;

				if (coupon == null)
				{
					discount = 0.0f;
					return sumItemTotal;
				}
				else if (coupon.CouponTypeId == 2)
				{
					discount = sumItemTotal - (float)(sumItemTotal - coupon.Amount * percent);

					return (float)(sumItemTotal - coupon.Amount * percent);
				}
				else
				{
					var couponAmount = totalOfAll * (coupon.Amount / 100);
					discount = sumItemTotal - (float)(sumItemTotal - couponAmount * percent);

					return (float)(sumItemTotal - couponAmount * percent);
				}

			}


			var orders = customerShoppingCartList.AsEnumerable().GroupBy(c => c.Item.ShopId).

			Select(g => new Order
			{
				OrderDate = DateTime.Now,
				OrderSerial = Guid.NewGuid().ToString() + "/" + DateTime.Now.Year,
				ShopId = g.Key.Value,
				CustomerId = checkOutViewModel.CustomerId,
				ShippingAddressId = checkOutViewModel.CustomerAddressId,
				OrderTotal = g.Sum(c => c.ItemTotal),
				CouponId = coupon != null ? checkOutViewModel.CouponId : null,
				CouponTypeId = coupon != null ? coupon.CouponTypeId : null,
				CouponAmount = coupon != null ? (float?)coupon.Amount : null,
				Deliverycost = _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
				OrderNet = calcOrderNet(g.Sum(c => c.ItemTotal)) + _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
				PaymentMehodPaymentMethodId = checkOutViewModel.PaymentMehodPaymentMethodId,
				OrderDiscount = discount,
			}).ToList();


			foreach (var item in orders)
			{
				_context.Order.Add(item);
				_context.SaveChanges();

				//transfer shoppingcart to orderitems table and clear shoppingcart


				List<OrderItem> orderItems = new List<OrderItem>();

				foreach (var itemshop in customerShoppingCartList)
				{
					if (itemshop.Item.ShopId == item.ShopId)
					{
						OrderItem orderItem = new OrderItem
						{
							ItemId = (int)itemshop.ItemId,
							ItemPrice = itemshop.ItemPrice,
							Total = itemshop.ItemTotal,
							ItemQuantity = itemshop.ItemQty,
							OrderId = item.OrderId
						};

						_context.OrderItems.Add(orderItem);
						_context.ShoppingCart.Remove(itemshop);
					}
				}

			}
			_context.SaveChanges();

			return (new { Success = true, message = "Order Succesfuly Added" });
		}

		[HttpGet]
		public Object GetCollectionsItems(int customerId, int countryId)
		{

			var lst = _context.Collections.Where(c => c.IsActive == true).OrderBy(c => c.Source)
				.Select(c => new
				{
					c.CollectionTitleEn,
					c.CollectionTitleAr,
					c.Source,
					Items = _context.CollectionItems
					.Include(c => c.Items)
					.ThenInclude(c => c.Shop)
					.Where(i => i.CollectionId == c.CollectionId && i.Items.Shop.CountryId == countryId)
					.Select(x => new
					{
						x.ItemId,
						x.Items.ItemPrice,
						x.Items.ItemNameAr,
						x.Items.ItemDescriptionAr,
						x.Items.ItemName,
						x.Items.ItemDescription,
						x.Items.Shop.ShopTlen,
						x.Items.Shop.ShopTlar,
						x.Items.Shop.ShopId,
						x.Items.OutOfStock,
						x.Items.CategoryId,
						ItemImage = "/images/" + x.Items.ItemImage,
						IsFavourite = customerId <= 0 ? false : _context.WishList.Any(w => w.CustomerId == customerId && w.Item.ItemId == x.Items.ItemId),

					}).ToList(),
				}).ToList();

			var collectionList = new List<Object>();

			foreach (var item in lst)
			{
				if (item.Items.Count() > 0)
				{
					collectionList.Add(item);
				}
			}
			return new { Collection = collectionList };


		}

		//[HttpGet]
		//public Object GetCollectionsItems(int customerId, int countryId)
		//{

		//        var lst = _context.Collections.Where(c => c.IsActive == true).OrderBy(c => c.Source)
		//            .Select(c => new {
		//                c.CollectionTitleEn,
		//                c.CollectionTitleAr,
		//                c.Source,
		//                Items = _context.CollectionItems
		//                .Include(c => c.Items)
		//                .Where(i => i.CollectionId == c.CollectionId && i.Items.CountryId == countryId)
		//                .Select(x => new
		//                {
		//                    x.ItemId,
		//                    x.Items.ItemPrice,
		//                    x.Items.ItemName,
		//                    x.Items.ItemDescription,
		//                    x.Items.Shop.ShopTlen,
		//                    x.Items.Shop.ShopTlar,
		//                    x.Items.Shop.ShopId,
		//                    x.Items.CategoryId,
		//                    ItemImage = "http://" + HttpContext.Request.Host + "/images/" + x.Items.ItemImage,
		//                    IsFavourite = customerId <= 0 ? false : _context.WishList.Any(w => w.CustomerId == customerId && w.Item.ItemId == x.Items.ItemId),

		//                }).ToList(),
		//            }).ToList();

		//    var collectionList = new List<Object>();

		//    foreach (var item in lst)
		//    {
		//        if(item.Items.Count() > 0)
		//        {
		//            collectionList.Add(item);
		//        }
		//    }
		//        return new { Collection = collectionList };


		//}

		[HttpGet]
		public Object GetCollections()
		{

			var lst = _context.Collections.OrderBy(c => c.Source).ToList();
			return new { Data = lst };
		}
		[HttpGet]
		// [Route("{CollectionId}")]
		public Object GetCollectionItemByCollectionId([FromQuery] int CollectionId)
		{
			var lst = _context.CollectionItems.Include(c => c.Items).Where(i => i.CollectionId == CollectionId).Select(x => new
			{
				x.ItemId,
				x.Items.ItemPrice,

				x.Items.ItemName,
				x.Items.ItemDescriptionAr,
				x.Items.ItemNameAr,
				x.Items.ItemDescription,
				ItemImage = "/images/" + x.Items.ItemImage
			}).ToList();



			return new { Collection = lst };
		}


		[HttpPost]
		public async Task<IActionResult> ChangePageContent(int key, [FromBody] string values)
		{
			var model = await _context.PageContent.FirstOrDefaultAsync(item => item.Id == key);
			if (model == null)
				return StatusCode(409, "Object not found");

			model.Content = values.ToString();
			await _context.SaveChangesAsync();
			return Ok();
		}


		[HttpPost]
		public async Task<IActionResult> AddSubscriber(string email)
		{
			_context.Newsletter.Add(new Newsletter()
			{
				Email = email
			});

			_context.SaveChanges();

			return Ok();
		}

		[HttpPost]
		public async Task<IActionResult> ContactUsMessages([FromBody] Gameapp.Models.ContactUsMessages contactDetails)
		{

			_context.ContactUsMessages.Add(contactDetails);
			_context.SaveChanges();
			return Ok();

		}

		[HttpGet]
		public IActionResult MakeNotificationIsRead([FromQuery] int PublicNotificationDeviceId)
		{

			try
			{
				var model = _context.PublicNotificationDevices.Find(PublicNotificationDeviceId);
				if (model == null)
				{
					return Ok(new { status = false, message = "Notification not Found" });


				}
				model.IsRead = true;
				_context.PublicNotificationDevices.Update(model);
				_context.SaveChanges();

				return Ok(new { status = true, message = "deviceId Added To trainer " });

			}
			catch (Exception)
			{

				return Ok(new { status = false, message = "Something went wrong" });

			}
		}
		[HttpPost]
		public IActionResult AddPublicDevice([FromBody] PublicDevice model)
		{

			try
			{
				var publicDevice = _context.PublicDevices.FirstOrDefault(c => c.DeviceId == model.DeviceId);
				if (publicDevice != null)
				{
					publicDevice.CountryId = model.CountryId;
					publicDevice.IsAndroiodDevice = model.IsAndroiodDevice;

					_context.PublicDevices.Update(publicDevice);
					_context.SaveChanges();
					return Ok(new { status = true, message = "deviceId edited" });
				}
				_context.PublicDevices.Add(model);
				_context.SaveChanges();
				return Ok(new { status = true, message = "deviceId Added" });

			}
			catch (Exception ex)
			{
				return Ok(new { status = false, message = ex });
			}
		}

		[HttpGet]
		public IActionResult GetPublicNotificationByDeviceId([FromQuery] string deviceId)
		{
			try
			{
				var publicDevice = _context.PublicDevices.FirstOrDefault(c => c.DeviceId == deviceId);
				var List = _context.PublicNotificationDevices.Include(c => c.PublicNotification).Where(c => c.PublicDeviceId == publicDevice.PublicDeviceId);


				return Ok(new { status = true, List });
			}
			catch (Exception)
			{

				return Ok(new { status = false, message = "Something went wrong" });

			}


		}
		[HttpGet]
		public IActionResult DeletePublicNotification([FromQuery] int publicNotificationDeviceId)
		{
			try
			{
				var model = _context.PublicNotificationDevices.FirstOrDefault(c => c.PublicNotificationDeviceId == publicNotificationDeviceId);
				_context.Remove(model);
				_context.SaveChanges();
				return Ok(new { status = true });
			}
			catch (Exception)
			{

				return Ok(new { status = false, message = "Something went wrong" });

			}


		}
	//    [HttpPost]
	//    public async Task<IActionResult> AddPayment([FromBody] PayOrderVm payOrderVm)
	//    {
	//        try
	//        {
	//            var CustomerObj = _context.Customer.Where(e => e.CustomerId == payOrderVm.CustomerId).FirstOrDefault();
	//            if (CustomerObj == null)
	//            {
	//                return Ok(new { status = false, message = "Enter Customer Id" });

	//            }
	//            var CustomerAddressObj = _context.ShippingAddress.Where(e => e.ShippingAddressId == payOrderVm.CustomerAddressId).FirstOrDefault();

	//            if (CustomerAddressObj == null)
	//            {
	//                return Ok(new { status = false, message = "Enter Customer Address Id" });


	//            }
	//            //var CouponObj = _context.Coupon.Where(e => e.Id == payOrderVm.CouponId).FirstOrDefault();

	//            //if (CouponObj == null)
	//            //{
	//            //    return Ok(new { status = false, message = "Enter Coupon Id" });


	//            //}
	//            var paymentMethod = _context.PaymentMehod.Where(e => e.PaymentMethodId == payOrderVm.PaymentMehodId).FirstOrDefault();

	//            if (paymentMethod == null)
	//            {
	//                return Ok(new { status = false, message = "Enter payment Method Id" });


	//            }

	//            float discount = 0.0f;


	//            //Get Customer ShoppingCart Items List
	//            var customerShoppingCartList = _context.
	//                ShoppingCart.Include(s => s.Customer)
	//                .Include(s => s.Item)
	//                .ThenInclude(s => s.Shop)
	//                .Where(c => c.CustomerId == payOrderVm.CustomerId);

	//            var totalOfAll = customerShoppingCartList.AsEnumerable().Sum(c => c.ItemTotal);

	//            // make coupon used

	//            Coupon coupon = null;
	//            coupon = _context.Coupon.FirstOrDefault(c => c.Id == payOrderVm.CouponId);


	//            //calc ordernet
	//            float calcOrderNet(float sumItemTotal)
	//            {
	//                var percent = sumItemTotal / totalOfAll;

	//                if (coupon == null)
	//                {
	//                    discount = 0.0f;
	//                    return sumItemTotal;
	//                }
	//                else if (coupon.CouponTypeId == 2)
	//                {
	//                    discount = sumItemTotal - (float)(sumItemTotal - coupon.Amount * percent);

	//                    return (float)(sumItemTotal - coupon.Amount * percent);
	//                }
	//                else
	//                {
	//                    var couponAmount = totalOfAll * (coupon.Amount / 100);
	//                    discount = sumItemTotal - (float)(sumItemTotal - couponAmount * percent);

	//                    return (float)(sumItemTotal - couponAmount * percent);
	//                }

	//            }

	//            int maxUniqe = 1;
	//            var newList = _context.Order.ToList();
	//            if (newList.Count > 0)
	//            {
	//                maxUniqe = newList.Max(e => e.uniqeId);
	//            }


	//            var orders = customerShoppingCartList.AsEnumerable().GroupBy(c => c.Item.ShopId).

	//            Select(g => new Order
	//            {
	//                OrderDate = DateTime.Now,
	//                OrderSerial = Guid.NewGuid().ToString() + "/" + DateTime.Now.Year,
	//                ShopId = g.Key.Value,
	//                CustomerId = payOrderVm.CustomerId,
	//                ShippingAddressId = payOrderVm.CustomerAddressId,
	//                OrderTotal = g.Sum(c => c.ItemTotal),
	//                CouponId = coupon != null ? payOrderVm.CouponId : null,
	//                CouponTypeId = coupon != null ? coupon.CouponTypeId : null,
	//                CouponAmount = coupon != null ? (float?)coupon.Amount : null,
	//                Deliverycost = _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
	//                OrderNet = calcOrderNet(g.Sum(c => c.ItemTotal)) + _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
	//                PaymentMehodPaymentMethodId = payOrderVm.PaymentMehodId,
	//                OrderDiscount = discount,
	//                uniqeId = maxUniqe + 1

	//            }).ToList();
	//            try
	//            {
	//                foreach (var item in orders)
	//                {
	//                    _context.Order.Add(item);
	//                    _context.SaveChanges();

	//                    //transfer shoppingcart to orderitems table and clear shoppingcart

	//                    List<OrderItem> orderItems = new List<OrderItem>();


	//                    foreach (var itemshop in customerShoppingCartList)
	//                    {
	//                        if (itemshop.Item.ShopId == item.ShopId)
	//                        {
	//                            OrderItem orderItem = new OrderItem
	//                            {
	//                                ItemId = (int)itemshop.ItemId,
	//                                ItemPrice = itemshop.ItemPrice,
	//                                Total = itemshop.ItemTotal,
	//                                ItemQuantity = itemshop.ItemQty,
	//                                OrderId = item.OrderId
	//                            };

	//                            _context.OrderItems.Add(orderItem);

	//                        }
	//                    }

	//                }
	//                _context.SaveChanges();


	//            }
	//            catch (Exception e)
	//            {
	//                return Ok(new { Staus = "false", message = e.Message });
	//            }



	//            var Customer = _context.Customer.Find(payOrderVm.CustomerId);
	//            if (Customer == null)
	//            {
	//                return Ok(new { Staus = "false", reason = "Enter Valid Customer" });
	//            }

	//            if (payOrderVm.PaymentMehodId == 2)
	//            {
	//                var requesturl = "https://api.upayments.com/test-payment";

	//                var fields = new
	//                {
	//                    merchant_id = "1201",
	//                    username = "test",
	//                    password = "test",
	//                    order_id = orders.FirstOrDefault().uniqeId,
	//                    total_price = orders.Sum(e => e.OrderNet),
	//                    test_mode = 0,
	//                    CstFName = Customer.CustomernameEn,
	//                    CstEmail = Customer.CustomerEmail,
	//                    CstMobile = Customer.Customerphone,
	//                    api_key = "jtest123",
	//                    success_url = "http://thegameskw.com/success",
	//                    error_url = "http://thegameskw.com/failed"

	//                };
	//                var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
	//                var task = httpClient.PostAsync(requesturl, content);
	//                var result = await task.Result.Content.ReadAsStringAsync();
	//                var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
	//                if (paymenturl.status == "success")

	//                {
	//                    return Ok(new { Staust = "true", paymenturl = paymenturl.paymentURL });
	//                }
	//                else
	//                {


	//                    _context.Order.Remove(order);
	//                    _context.SaveChanges();
	//                    return Ok(new { Staus = "false", reason = paymenturl.error_msg });


	//                }
	//            }
	//            else if (payOrderVm.PaymentMehodId == 3)
	//            {

	//                bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
	//                var TestToken = _configuration["TestToken"];
	//                var LiveToken = _configuration["LiveToken"];
	//                if (Fattorahstatus) // fattorah live
	//                {
	//                    var sendPaymentRequest = new
	//                    {

	//                        CustomerName = Customer.CustomernameEn,
	//                        NotificationOption = "LNK",
	//                        InvoiceValue = orders.Sum(e => e.OrderNet),
	//                        CallBackUrl = "http://thegameskw.com/FattorahSuccess",
	//                        ErrorUrl = "http://thegameskw.com/FattorahError",
	//                        UserDefinedField = orders.FirstOrDefault().uniqeId,
	//                        CustomerEmail = Customer.CustomerEmail

	//                    };
	//                    var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

	//                    string url = "https://apitest.myfatoorah.com/v2/SendPayment";
	//                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	//                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
	//                    var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
	//                    var responseMessage = httpClient.PostAsync(url, httpContent);
	//                    var res = await responseMessage.Result.Content.ReadAsStringAsync();
	//                    var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);
	//                    if (FattoraRes.IsSuccess == true)
	//                    {
	//                        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
	//                        var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
	//                        return Ok(new { Staust = "true", paymenturl = InvoiceRes.InvoiceURL });



	//                    }
	//                    else
	//                    {
	//                        return Ok(new { Staus = "false", reason = FattoraRes.Message });

	//                    }
	//                }
	//                else               // fattorah test
	//                {
	//                    var sendPaymentRequest = new
	//                    {

	//                        CustomerName = Customer.CustomernameEn,
	//                        NotificationOption = "LNK",
	//                        InvoiceValue = orders.Sum(e => e.OrderNet),
	//                        CallBackUrl = "http://thegameskw.com/FattorahSuccess",
	//                        ErrorUrl = "http://thegameskw.com/FattorahError",
	//                        UserDefinedField = orders.FirstOrDefault().uniqeId,
	//                        CustomerEmail = Customer.CustomerEmail

	//                    };
	//                    var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

	//                    string url = "https://apitest.myfatoorah.com/v2/SendPayment";
	//                    httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
	//                    httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
	//                    var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
	//                    var responseMessage = httpClient.PostAsync(url, httpContent);
	//                    var res = await responseMessage.Result.Content.ReadAsStringAsync();
	//                    var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);
	//                    if (FattoraRes.IsSuccess == true)
	//                    {
	//                        Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
	//                        var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
	//                        return Ok(new { Staust = "true", paymenturl = InvoiceRes.InvoiceURL });



	//                    }
	//                    else
	//                    {
	//                        return Ok(new { Staus = "false", reason = FattoraRes.Message });

	//                    }

	//                }
	//            }
	//                if (payOrderVm.PaymentMehodId == 1)
	//                {

	//                    if (orders.FirstOrDefault() == null)
	//                    {
	//                        return Ok(new { Staust = "false", message = "No Orders" });

	//                    }
	//                    if (coupon != null)
	//                    {
	//                        coupon.Used = true;
	//                        var UpdatedCoupon = _context.Coupon.Attach(coupon);
	//                        UpdatedCoupon.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
	//                    }

	//                    _context.ShoppingCart.RemoveRange(customerShoppingCartList);
	//                    _context.SaveChanges();
	//                    return Ok(new { Staust = "true", paymenturl = "http://thegameskw.com/Thankyou" });




	//                }

	//            }
	//catch (Exception ex)
	//        {
	//            return Ok(new { Staust = "false", reason = ex.Message });

	//        }
	//        return Ok();


	//    }
		#region Champions
		[HttpGet]
		public IActionResult GetAllPlatForms()
		{
			try
			{
				var Platforms = _context.Platforms.ToList();
				return Ok(new { status = true, PlatformsList = Platforms });

			}
			catch (Exception e)
			{

				return Ok(new { status = false, message = e.Message });

			}


		}
		[HttpGet]
		public IActionResult GetGamesByPlatFormId(int platformId)
		{
			try
			{
				var Platforms = _context.PlatformGames.Where(e => e.PlatFormId == platformId).ToList();
				return Ok(new { status = true, PlatformsList = Platforms });

			}
			catch (Exception e)
			{

				return Ok(new { status = false, message = e.Message });

			}


		}
		[HttpGet]
		public IActionResult GetAllChampionsType()
		{
			try
			{
				var ChampionsType = _context.ChampionType.Select(c =>
						new
						{
							ChampionId = c.Id,
							ChampionTlAR = c.NameAr,
							ChampionTlEn = c.NameEn,
							Champions = _context.Champions.Include(c => c.GameMode).Where(cc => cc.ChampTypeId == c.Id).ToList()
						}).ToList();
				return Ok(new { status = true, ChampionsType = ChampionsType });

			}
			catch (Exception e)
			{

				return Ok(new { status = false, message = e.Message });

			}


		}
		[HttpGet]
		public IActionResult GetChampionListByTypeId(int typeId)
		{
			try
			{
				var champions =
				 _context.Champions.Include(c => c.GameMode).Where(c => c.ChampTypeId == typeId)
					.Select(c =>
						new
						{
							ChampionId = c.ChampionId,
							ChampionTlAR = c.ChampionTlAR,
							ChampionTlEn = c.ChampionTlEn,
							StartDate = c.StartDate,
							EndDate = c.EndDate,
							JoinStart = c.JoinStart,
							JoinEnd = c.JoinEnd,
							ChampionPic = "/images/" + c.ChampionPic,
							ChampionContent = c.ChampionContent,
							GameModelId = c.GameModeId,
							GameMode = c.GameMode,
							ChampionParticipates = _context.ChampionParticipates.Include(s => s.Customer).Where(cc => cc.ChampionId == c.ChampionId).ToList(),
						}).ToList();

				return Ok(new { status = true, championsList = champions });

			}
			catch (Exception e)
			{

				return Ok(new { status = false, message = e.Message });

			}


		}
		[HttpPost]
		[Route("{champId}/{customerId}")]
		public ActionResult<object> RegsisterParticpates(int champId, int customerId)
		{
			try
			{
				var Champion = _context.Champions.Where(e => e.ChampionId == champId).FirstOrDefault();
				if (Champion != null)
				{
					var customerIsPatrticipate = _context.ChampionParticipates.Where(e => e.CustomerId == customerId).FirstOrDefault();
					if (customerIsPatrticipate != null)
					{
						return new { Status = false, Message = "You are Aleardy Participated In This Champion" };

					}
					if (DateTime.Now >= Champion.JoinStart && Champion.JoinEnd >= DateTime.Now)
					{
						var regsisterParticipates = new ChampionParticipate()
						{
							ChampionId = champId,
							CustomerId = customerId,
							JoinDate = DateTime.Now,
							ParticipStateId = 1
						};
						_context.ChampionParticipates.Add(regsisterParticipates);
						_context.SaveChanges();
						return new { Status = true, Obj = regsisterParticipates };

					}
					return new { Status = false, Message = "Can Not Partcipate In This Champion Becouse It Ended Or Running" };
				}
				return new { Status = false, Message = "Champion Not Found" };


			}
			catch (Exception ex)
			{
				return new { Status = false, Message = ex.Message };
			}

		}
		[HttpGet]
		public IActionResult GetAllPrticipationChampionsByCustomerId(int customerId)
		{
			try
			{
				var PrticipationChampionsList = _context.ChampionParticipates.Include(e => e.Champion).Include(c => c.Customer).Include(e => e.ParticipState).Where(e => e.CustomerId == customerId).ToList();
				return Ok(new { status = true, PrticipationChampionsList = PrticipationChampionsList });

			}
			catch (Exception e)
			{

				return Ok(new { status = false, message = e.Message });

			}


		}
		[HttpGet]
		public IActionResult GetPrticipationChampionsCountByCustomerId(int customerId)
		{
			try
			{
				int ChampionsCount = _context.ChampionParticipates.Where(e => e.CustomerId == customerId).Count();
				return Ok(new { status = true, ChampionsCount = ChampionsCount });

			}
			catch (Exception e)
			{

				return Ok(new { status = false, message = e.Message });

			}


		}
		#endregion

		[HttpDelete]
		public IActionResult DeleteCustomerAccount(int customerId)
		{
			var user = _db.ApplicationUsers.Where(e => e.EntityId == customerId).FirstOrDefault();
			try
			{
				if (user == null)
				{
					return Ok(new { Status = false, Message = "Customer Not Found" });
				}

				_db.Users.Remove(user);
				_db.SaveChanges();
			}
			catch (Exception e)
			{
				return Ok(new { Status = false, Message = e.Message });

			}
			return Ok(new { Status = true, Message = "Customer Account Deleted Successfully" });
		}
		[HttpGet]
		public IActionResult GetAllPaymentMethods()
		{
			try
			{
			   var PaymentMethods= _context.PaymentMehod.Where(e => e.IsActive == true)
				.Select(c => new
				{
					PaymentMethodId = c.PaymentMethodId,
					PaymentMethodName = c.PaymentMethodName,
					PaymentPaymentMethodPic = "/Images/PaymentMethod/" + c.PaymentMethodPic
				});
				return Ok(new { Status = true, PaymentMethods = PaymentMethods });

			}
			catch (Exception e)
			{

				return Ok(new { Status = false, message = e.Message });

			}
		   

		}
		//[HttpPost]
		//public async Task<IActionResult> CheckOut(CheckOutVM checkOutVM)
		//{
		//    try
		//    {
		//        var customerObj = _context.Customers.FirstOrDefault(c => c.CustomerId == checkOutVM.CustomerId);
		//        if (customerObj == null)
		//        {
		//            return Ok(new { status = false, message = "Customer Object Not Found" });

		//        }
		//        var customerAddressObj = _context.CustomerAddresses.FirstOrDefault(c => c.CustomerAddressId == checkOutVM.CustomerAddressId);
		//        if (customerAddressObj == null)
		//        {
		//            return Ok(new { status = false, message = "Customer Address Object Not Found" });

		//        }
		//        var paymentObj = _context.PaymentMehods.FirstOrDefault(c => c.PaymentMethodId == checkOutVM.PaymentMethodId);
		//        if (paymentObj == null)
		//        {
		//            return Ok(new { status = false, message = "Payment Object Not Found" });

		//        }
		//        // return Ok(new { status = true, shopList = shopList });



		//        double discount = 0;


		//        //Get Customer ShoppingCart Items List
		//        var customerShoppingCartList = _context.
		//            ShoppingCarts.Include(s => s.Customer)
		//            .Include(s => s.Item)
		//            .ThenInclude(s => s.Shop)
		//            .Where(c => c.CustomerId == checkOutVM.CustomerId);

		//        var totalOfAll = customerShoppingCartList.AsEnumerable().Sum(c => c.ItemTotal);

		//        // make coupon used

		//        Coupon coupon = null;
		//        coupon = _context.Coupons.FirstOrDefault(c => c.CouponId == checkOutVM.CouponId);


		//        //calc ordernet
		//        double calcOrderNet(double sumItemTotal)
		//        {
		//            var percent = sumItemTotal / totalOfAll;

		//            if (coupon == null)
		//            {
		//                discount = 0;
		//                return sumItemTotal;
		//            }
		//            else if (coupon.CouponTypeId == 2)
		//            {
		//                discount = sumItemTotal - (float)(sumItemTotal - coupon.Amount * percent);

		//                return (float)(sumItemTotal - coupon.Amount * percent);
		//            }
		//            else
		//            {
		//                var couponAmount = totalOfAll * (coupon.Amount / 100);
		//                discount = sumItemTotal - (float)(sumItemTotal - couponAmount * percent);

		//                return (float)(sumItemTotal - couponAmount * percent);
		//            }

		//        }

		//        int maxUniqe = 1;
		//        var newList = _context.Orders.ToList();
		//        if (newList.Count > 0)
		//        {
		//            maxUniqe = newList.Max(e => e.UniqeId);
		//        }


		//        var orders = customerShoppingCartList.AsEnumerable().GroupBy(c => c.Item.ShopId).

		//        Select(g => new Order
		//        {
		//            OrderDate = DateTime.Now,
		//            OrderSerial = Guid.NewGuid().ToString() + "/" + DateTime.Now.Year,
		//            ShopId = g.Key,
		//            CustomerId = checkOutVM.CustomerId,
		//            CustomerAddressId = checkOutVM.CustomerAddressId,
		//            OrderTotal = g.Sum(c => c.ItemTotal),
		//            CouponId = coupon != null ? checkOutVM.CouponId : null,
		//            CouponTypeId = coupon != null ? coupon.CouponTypeId : null,
		//            CouponAmount = coupon != null ? (float?)coupon.Amount : null,
		//            Deliverycost = _context.Shops.FirstOrDefault(s => s.ShopId == g.Key).DeliveryCost,
		//            OrderNet = calcOrderNet(g.Sum(c => c.ItemTotal)) + _context.Shops.FirstOrDefault(s => s.ShopId == g.Key).DeliveryCost,
		//            PaymentMethodId = checkOutVM.PaymentMethodId,
		//            OrderDiscount = discount,
		//            UniqeId = maxUniqe + 1

		//        }).ToList();



		//        foreach (var item in orders)
		//        {
		//            _context.Orders.Add(item);
		//            _context.SaveChanges();

		//            //transfer shoppingcart to orderitems table and clear shoppingcart

		//            List<OrderItem> orderItems = new List<OrderItem>();


		//            foreach (var itemshop in customerShoppingCartList)
		//            {
		//                if (itemshop.Item.ShopId == item.ShopId)
		//                {
		//                    OrderItem orderItem = new OrderItem
		//                    {
		//                        ItemId = (int)itemshop.ItemId,
		//                        ItemPrice = itemshop.ItemPrice,
		//                        Total = itemshop.ItemTotal,
		//                        ItemQuantity = itemshop.ItemQty,
		//                        OrderId = item.OrderId
		//                    };

		//                    _context.OrderItems.Add(orderItem);

		//                }
		//            }

		//        }
		//        _context.SaveChanges();
		//        double pointCost = 0;
		//        var point = _context.Points.ToList().Take(1).FirstOrDefault();
		//        if (point != null)
		//        {
		//            pointCost = point.AmountOfOnePoint * customerObj.NumberOfPoints;

		//        }


		//        if (checkOutVM.PaymentMethodId == 1)
		//        {

		//            bool Fattorahstatus = bool.Parse(_configuration["FattorahStatus"]);
		//            var TestToken = _configuration["TestToken"];
		//            var LiveToken = _configuration["LiveToken"];
		//            if (Fattorahstatus) // fattorah live
		//            {
		//                var sendPaymentRequest = new
		//                {

		//                    CustomerName = customerObj.NameAr,
		//                    NotificationOption = "LNK",
		//                    InvoiceValue = orders.Sum(e => e.OrderNet) - pointCost,
		//                    CallBackUrl = "http://dajeejapp.com/FattorahOrderSuccess",
		//                    ErrorUrl = "http://dajeejapp.com/FattorahOrderFaild",
		//                    UserDefinedField = orders.FirstOrDefault().UniqeId,
		//                    CustomerEmail = customerObj.Email

		//                };
		//                var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

		//                string url = "https://api.myfatoorah.com/v2/SendPayment";
		//                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		//                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", LiveToken);
		//                var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
		//                var responseMessage = httpClient.PostAsync(url, httpContent);
		//                var res = await responseMessage.Result.Content.ReadAsStringAsync();
		//                var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);

		//                if (FattoraRes.IsSuccess == true)
		//                {
		//                    Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
		//                    var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();
		//                    return Ok(new { status = true, Url = InvoiceRes.InvoiceURL });

		//                }
		//                else
		//                {
		//                    return Ok(new { status = false, Message = FattoraRes.Message });
		//                }
		//            }
		//            else               // fattorah test
		//            {
		//                var sendPaymentRequest = new
		//                {

		//                    CustomerName = customerObj.NameAr,
		//                    NotificationOption = "LNK",
		//                    InvoiceValue = orders.Sum(e => e.OrderNet) - pointCost,
		//                    CallBackUrl = "http://dajeejapp.com/FattorahOrderSuccess",
		//                    ErrorUrl = "http://dajeejapp.com/FattorahOrderFaild",
		//                    UserDefinedField = orders.FirstOrDefault().UniqeId,
		//                    CustomerEmail = customerObj.Email

		//                };
		//                var sendPaymentRequestJSON = JsonConvert.SerializeObject(sendPaymentRequest);

		//                string url = "https://apitest.myfatoorah.com/v2/SendPayment";
		//                httpClient.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
		//                httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", TestToken);
		//                var httpContent = new StringContent(sendPaymentRequestJSON, Encoding.UTF8, "application/json");
		//                var responseMessage = httpClient.PostAsync(url, httpContent);
		//                var res = await responseMessage.Result.Content.ReadAsStringAsync();
		//                var FattoraRes = JsonConvert.DeserializeObject<FattorhResult>(res);
		//                if (FattoraRes.IsSuccess == true)
		//                {
		//                    Newtonsoft.Json.Linq.JObject jObject = Newtonsoft.Json.Linq.JObject.Parse(res);
		//                    var InvoiceRes = jObject["Data"].ToObject<InvoiceData>();

		//                    return Ok(new { status = true, Url = InvoiceRes.InvoiceURL });

		//                }
		//                else
		//                {
		//                    return Ok(new { status = false, Message = FattoraRes.Message });
		//                }
		//            }


		//        }
		//        if (checkOutVM.PaymentMethodId == 2)
		//        {

		//            if (orders.FirstOrDefault().CustomerId != null)
		//            {
		//                if (coupon != null)
		//                {
		//                    coupon.Used = true;
		//                    var UpdatedCoupon = _context.Coupons.Attach(coupon);
		//                    UpdatedCoupon.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
		//                }

		//                _context.ShoppingCarts.RemoveRange(customerShoppingCartList);
		//                _context.SaveChanges();
		//                return Ok(new { status = true, Url = "http://dajeejapp.com/Thankyou" });

		//            }

		//        }
		//        return Ok();

		//    }

		//    catch (Exception ex)
		//    {

		//        return Ok(new { status = false, message = ex.Message });

		//    }

		//}
		[HttpPost]
		public async Task<IActionResult> AddPayment([FromBody] PayOrderVm payOrderVm)
		{
			try
			{
				var CustomerObj = _context.Customer.Where(e => e.CustomerId == payOrderVm.CustomerId).FirstOrDefault();
				if (CustomerObj == null)
				{
					return Ok(new { status = false, message = "Enter Customer Id" });

				}
				var CustomerAddressObj = _context.ShippingAddress.Where(e => e.ShippingAddressId == payOrderVm.CustomerAddressId).FirstOrDefault();

				if (CustomerAddressObj == null)
				{
					return Ok(new { status = false, message = "Enter Customer Address Id" });


				}
				//var CouponObj = _context.Coupon.Where(e => e.Id == payOrderVm.CouponId).FirstOrDefault();

				//if (CouponObj == null)
				//{
				//    return Ok(new { status = false, message = "Enter Coupon Id" });


				//}
				var paymentMethod = _context.PaymentMehod.Where(e => e.PaymentMethodId == payOrderVm.PaymentMehodId).FirstOrDefault();

				if (paymentMethod == null)
				{
					return Ok(new { status = false, message = "Enter payment Method Id" });


				}

				float discount = 0.0f;


				//Get Customer ShoppingCart Items List
				var customerShoppingCartList = _context.
					ShoppingCart.Include(s => s.Customer)
					.Include(s => s.Item)
					.ThenInclude(s => s.Shop)
					.Where(c => c.CustomerId == payOrderVm.CustomerId);

				var totalOfAll = customerShoppingCartList.AsEnumerable().Sum(c => c.ItemTotal);

				// make coupon used

				Coupon coupon = null;
				coupon = _context.Coupon.FirstOrDefault(c => c.Id == payOrderVm.CouponId);


				//calc ordernet
				float calcOrderNet(float sumItemTotal)
				{
					var percent = sumItemTotal / totalOfAll;

					if (coupon == null)
					{
						discount = 0.0f;
						return sumItemTotal;
					}
					else if (coupon.CouponTypeId == 2)
					{
						discount = sumItemTotal - (float)(sumItemTotal - coupon.Amount * percent);

						return (float)(sumItemTotal - coupon.Amount * percent);
					}
					else
					{
						var couponAmount = totalOfAll * (coupon.Amount / 100);
						discount = sumItemTotal - (float)(sumItemTotal - couponAmount * percent);

						return (float)(sumItemTotal - couponAmount * percent);
					}

				}

				int maxUniqe = 1;
				var newList = _context.Order.ToList();
				if (newList.Count > 0)
				{
					maxUniqe = newList.Max(e => e.uniqeId);
				}


				var orders = customerShoppingCartList.AsEnumerable().GroupBy(c => c.Item.ShopId).

				Select(g => new Order
				{
					OrderDate = DateTime.Now,
					OrderSerial = Guid.NewGuid().ToString() + "/" + DateTime.Now.Year,
					ShopId = g.Key.Value,
					CustomerId = payOrderVm.CustomerId,
					ShippingAddressId = payOrderVm.CustomerAddressId,
					OrderTotal = g.Sum(c => c.ItemTotal),
					CouponId = coupon != null ? payOrderVm.CouponId : null,
					CouponTypeId = coupon != null ? coupon.CouponTypeId : null,
					CouponAmount = coupon != null ? (float?)coupon.Amount : null,
					Deliverycost = _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
					OrderNet = calcOrderNet(g.Sum(c => c.ItemTotal)) + _context.Shop.FirstOrDefault(s => s.ShopId == g.Key.Value).Deliverycost,
					PaymentMehodPaymentMethodId = payOrderVm.PaymentMehodId,
					OrderDiscount = discount,
					uniqeId = maxUniqe + 1

				}).ToList();
				try
				{
					foreach (var item in orders)
					{
						_context.Order.Add(item);
						_context.SaveChanges();

						//transfer shoppingcart to orderitems table and clear shoppingcart

						List<OrderItem> orderItems = new List<OrderItem>();


						foreach (var itemshop in customerShoppingCartList)
						{
							if (itemshop.Item.ShopId == item.ShopId)
							{
								OrderItem orderItem = new OrderItem
								{
									ItemId = (int)itemshop.ItemId,
									ItemPrice = itemshop.ItemPrice,
									Total = itemshop.ItemTotal,
									ItemQuantity = itemshop.ItemQty,
									OrderId = item.OrderId
								};

								_context.OrderItems.Add(orderItem);

							}
						}

					}
					_context.SaveChanges();


				}
				catch (Exception e)
				{
					return Ok(new { Staus = "false", message = e.Message });
				}



				var Customer = _context.Customer.Find(payOrderVm.CustomerId);
				if (Customer == null)
				{
					return Ok(new { Staus = "false", reason = "Enter Valid Customer" });
				}

				if (payOrderVm.PaymentMehodId == 2)
				{
					var requesturl = "https://api.upayments.com/test-payment";

					var fields = new
					{
						merchant_id = "1201",
						username = "test",
						password = "test",
						order_id = orders.FirstOrDefault().uniqeId,
						total_price = orders.Sum(e => e.OrderNet),
						test_mode = 0,
						CstFName = Customer.CustomernameEn,
						CstEmail = Customer.CustomerEmail,
						CstMobile = Customer.Customerphone,
						api_key = "jtest123",
						success_url = "http://thegameskw.com/success",
						error_url = "http://thegameskw.com/failed"

					};
					var content = new StringContent(JsonConvert.SerializeObject(fields), Encoding.UTF8, "application/json");
					var task = httpClient.PostAsync(requesturl, content);
					var result = await task.Result.Content.ReadAsStringAsync();
					var paymenturl = JsonConvert.DeserializeObject<paymenturl>(result);
					if (paymenturl.status == "success")

					{
						return Ok(new { Staust = "true", paymenturl = paymenturl.paymentURL, OrderId = order.OrderId });
					}
					else
					{


						_context.Order.Remove(order);
						_context.SaveChanges();
						return Ok(new { Staus = "false", reason = paymenturl.error_msg });


					}
				}
				else if (payOrderVm.PaymentMehodId == 3)
				{
					bool TapStatus = bool.Parse(_configuration["TapStatus"]);
					var TestToken = _configuration["TestToken"];
					var LiveToken = _configuration["LiveToken"];
					if (TapStatus) // Tap Payment live
					{
						var TapMessage = new
						{
							amount = orders.Sum(e => e.OrderNet),
							currency = "KWD",
							threeDSecure = true,
							save_card = false,
							description = "Games Site Fees",
							statement_descriptor = "Sample",
							metadata = new
							{
								udf1 = Customer.CustomerId,
								udf2 = Customer.CustomernameEn,
							},
							reference = new
							{
								transaction = "txn_0001",
								order = orders.FirstOrDefault().uniqeId
							},
							receipt = new
							{
								email = false,
								sms = true
							},
							customer = new
							{
								first_name = Customer.CustomernameEn,
								middle_name = "test",
								last_name = "test",
								email = Customer.CustomerEmail,

                                phone = new
                                {
                                    country_code = "965",
                                    //number = Customer.Customerphone
                                     number = "50143413"
                                   // number = "50000000"

                                }
                            },

							//merchant = new { id = "194729" },
							merchant = new { id = "" },
							source = new { id = "src_kw.knet" },
							redirect = new { url = "https://thegameskw.com/CallBack" },
							//redirect = new { url = "https://localhost:44338/CallBack" },
							//post= new { url = "https://thegameskw.com/CallBack" },
						};

						var sendPaymentRequestJSON = JsonConvert.SerializeObject(TapMessage);

						var client = new RestClient("https://api.tap.company/v2/charges");
						var request = new RestRequest();
						request.AddHeader("content-type", "application/json");
						request.AddHeader("authorization", LiveToken);
						request.AddParameter("application/json", sendPaymentRequestJSON, ParameterType.RequestBody);
						RestResponse response = await client.PostAsync(request);

						var DeserializeObjectResopnse = JsonConvert.DeserializeObject<JObject>(response.Content);

						var Transaction = DeserializeObjectResopnse.GetValue("transaction");

						var Url = Transaction["url"].ToString();
						return Ok(new { Staust = "true", paymenturl = Url,OrderId= orders.FirstOrDefault().uniqeId });

					}
					else               // Tap Payment test
					{
						var TapMessage = new
						{
							amount = orders.Sum(e => e.OrderNet),
							currency = "KWD",
							threeDSecure = true,
							save_card = false,
							description = "Games Site Fees",
							statement_descriptor = "Sample",
							metadata = new
							{
								udf1 = Customer.CustomerId,
								udf2 = Customer.CustomernameEn,
							},
							reference = new
							{
								transaction = "txn_0001",
								order = orders.FirstOrDefault().uniqeId
							},
							receipt = new
							{
								email = false,
								sms = true
							},
							customer = new
							{
								first_name = Customer.CustomernameEn,
								middle_name = "test",
								last_name = "test",
								email = Customer.CustomerEmail,

                                phone = new
                                {
                                    country_code = "965",
                                    //number = Customer.Customerphone
                                    number = "50143413"
                                    //number = "50000000"
                                }
                            },
							//merchant = new { id = "194729" },
							merchant = new { id = "" },
							source = new { id = "src_kw.knet" },
							//post = new { url = "https://thegameskw.com/CallBack" },
							redirect = new { url = "https://thegameskw.com/CallBack" }
							//redirect = new { url = "https://localhost:44338/CallBack" }
						};

						var sendPaymentRequestJSON = JsonConvert.SerializeObject(TapMessage);

						var client = new RestClient("https://api.tap.company/v2/charges");
						var request = new RestRequest();
						request.AddHeader("content-type", "application/json");
						request.AddHeader("authorization", TestToken);
						request.AddParameter("application/json", sendPaymentRequestJSON, ParameterType.RequestBody);
						RestResponse response = await client.PostAsync(request);

						var DeserializeObjectResopnse = JsonConvert.DeserializeObject<JObject>(response.Content);

						var Transaction = DeserializeObjectResopnse.GetValue("transaction");

						var Url = Transaction["url"].ToString();
						return Ok(new { Staust = "true", paymenturl = Url, OrderId = orders.FirstOrDefault().uniqeId });

					}

					
				}
				if (payOrderVm.PaymentMehodId == 1)
				{

					if (orders.FirstOrDefault() == null)
					{
						return Ok(new { Staust = "false", message = "No Orders" });

					}
					if (coupon != null)
					{
						coupon.Used = true;
						var UpdatedCoupon = _context.Coupon.Attach(coupon);
						UpdatedCoupon.State = Microsoft.EntityFrameworkCore.EntityState.Modified;
					}

					_context.ShoppingCart.RemoveRange(customerShoppingCartList);
					_context.SaveChanges();
					return Ok(new { Staust = "true", paymenturl = "https://thegameskw.com/Thankyou", OrderId = orders.FirstOrDefault().uniqeId });




				}

			}
			catch (Exception ex)
			{
				return Ok(new { Staust = "false", reason = ex.Message });

			}
			return Ok();


		}
		[HttpGet]
		public IActionResult GetOrderStatustByUniqueId(int UniqueId)
		{
			try
			{
				var order =  _context.Order.Where(e => e.uniqeId == UniqueId).FirstOrDefault();
				if (order == null)
				{
					return Ok(new { Status = false, Message = "Order No Found" });
				}
				return Ok(new { Status = true, IsPaid = order.ispaid });

			}
			catch (Exception e)
			{

				return Ok(new { Status = false, Message = e.Message });

			}


		}
	}
}


