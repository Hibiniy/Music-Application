﻿using Lab7Task1.DAL;
using Lab7Task1.Models;
using Lab7Task1.ViewModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace lab7.Controllers
{
    public class ShoppingCartController : Controller
    {
        MusicStoreEntities storeDB = new MusicStoreEntities();
        //
        // GET: /ShoppingCart/
        public ActionResult Index()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            // Set up our ViewModel
            var viewModel = new ShoppingCartViewModel
            {
                CartItems = cart.GetCartItems(),
                CartTotal = cart.GetTotal()
            };
            return View(viewModel);
        }
        //
        // GET: /Store/AddToCart/5
        public ActionResult AddToCart(int id)
        {
            var addedAlbum = storeDB.Albums
            .Single(album => album.AlbumId == id);
            var cart = ShoppingCart.GetCart(this.HttpContext);
            cart.AddToCart(addedAlbum);
            return RedirectToAction("Index");
        }

        [HttpPost]
        public ActionResult RemoveFromCart(int id)
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);
            string albumName = storeDB.Carts
            .Single(item => item.RecordId == id).Album.Title;
            int itemCount = cart.RemoveFromCart(id);
            var results = new ShoppingCartRemoveViewModel()
            {
                Message = Server.HtmlEncode(albumName) +
                "has been removed from your shopping cart.",
                CartTotal = cart.GetTotal(),
                CartCount = cart.GetCount(),
                ItemCount = itemCount,
                DeleteId = id
            };
            return Json(results);
        }

        [ChildActionOnly]
        public ActionResult CartSummary()
        {
            var cart = ShoppingCart.GetCart(this.HttpContext);

            ViewData["CartCount"] = cart.GetCount();
            return PartialView("CartSummary");
        }
    }
}