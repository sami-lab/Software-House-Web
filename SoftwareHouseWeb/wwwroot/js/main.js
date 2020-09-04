$(document).ready(function(){
    
    ////switch//

var orderBtn = $('#order-btn')
orderBtn.click(function(){
    location = '../ourpackages.php'
})


$(".nav-items").children().click(function(){
    if(this.id === "logo")
    {
        window.location = "../index.html"
    }
    else if(this.id === "webdesign")
    {
        window.location = "../webdesign/webdesign.html"
    }
    else if(this.id === "webdev")
    {
    window.location = "../webdevelopment/webdev.html"
    }
})
$(".nav-items").children().on("mouseenter",function(){
    var navitemexpand = $(".nav-item-expand");
if(this.id === "logo"){
    navitemexpand.css("left","15%")
    $(navitemexpand).children().click(function(){
        if(this.className === "portfolio"){
            window.location ='../logodesign/Portfolio.html'      
        }
        else if(this.className === "our-packages"){
            location = `../logodesign/packages.html`
        }
        else if(this.className === "how-it-works")
        {
            window.location ='../logodesign/howitworks.html'      

        }
       
    })
    $(".nav-item-expand").css("display","grid")
    $(".nav-item-expand").animate({opacity:'1'},500)
}
else if(this.id === "webdesign")
{
    navitemexpand.css("left","25%")
    $(navitemexpand).children().click(function(){
        if(this.className === "portfolio"){
            window.location ='../webdesign/Portfolio.html'      
        }
        else if(this.className === "our-packages"){
            location = `../webdesign/packages.html`
        }
        else if(this.className === "how-it-works")
        {
            window.location ='../webdesign/howitworks.html'      

        }
    })
    $(".nav-item-expand").css("display","grid")
    $(".nav-item-expand").animate({opacity:'1'},500)
}
else if(this.id === "webdev"){
    navitemexpand.css("left","37.5%")
    $(navitemexpand).children().click(function(){
        if(this.className === "portfolio"){
            window.location ='../webdevelopment/portfolio.html'      
        }
        else if(this.className === "our-packages"){
            location = `../webdevelopment/packages.html`
        }
        else if(this.className === "how-it-works")
        {
            window.location ='../webdevelopment/howitworks.html'      

        }
    })
    $(".nav-item-expand").css("display","grid")
    $(".nav-item-expand").animate({opacity:'1'},200)
}


  
})
$(".nav-item-expand").mouseleave(function(){
    $(".nav-item-expand").animate({opacity:'0'},200)
    setTimeout(() => {
    $(".nav-item-expand").css("display","none");
        
    }, 200);

})

//mobile nav
var menuDropdown = $('.data-dropdown')
menuDropdown.slideUp();
$(".dropdown").click(function(){
    // console.log()
   var x = $(this).siblings()[0]
   $(x).slideToggle()
})
$('.ham img').click(function(){
    $('.mobile-menu').removeClass('menu-off')

    $('.mobile-menu').addClass('menu-on')
})
$('.cross img').click(function(){
    $('.mobile-menu').removeClass('menu-on')

    $('.mobile-menu').addClass('menu-off')
})

//////////////////////////////////////////////

var slide = false;
$("#chat-btn").click(function(){
    if(slide === false){
        $(".chat-box").animate({height:"50rem"},300)
    slide = true;
    }
    else{
        $(".chat-box").animate({height:"5.5rem"},300)
        slide = false;
    }
})
function loaderOff(){
    $(".wrapper").css("display","none")
    console.log("off")
}
loaderOff();
$('.goto-testimonial').click(()=>{
    location = '../testimonials/tt.html'
})
var testSlide = $('.test-slide')
var arrayOfcards =testSlide.children()
var i = -1
var fn = (x)=>{
 $(x).css('transition','300ms ease')
 $(x).css('opacity','1')
 setTimeout(() => {
    $(x).css('transition','300ms ease')
 $(x).css('opacity','0')
 }, 2000);
}

setInterval(()=>{
 i++;
 if(i ===  arrayOfcards.length){
    i = 0;
    fn(arrayOfcards[i])
   
 }
 else{
    fn(arrayOfcards[i])

 }
},3000)
})

///////////////////////////
////////////stickynavbar/////////

$(window).scroll(function(){
    var height_scroll = $(document).scrollTop();
    var navbar = $(".sticky-navbar");
    // console.log(height)
    if(height_scroll >= "125"){
        navbar.css("display","grid")
        
    }

    else if(height_scroll < 125)
    {
       
        $(".sticky-navbar").css("display","none")
        $(".nav-item-expand-sticky").css("display","none")
        
    }
    $(".sticky-items").children().on("mouseenter",function(){
       
    if(this.id === "logo2"){
        $(".nav-item-expand-sticky").css("left","26%")
        $('.nav-item-expand-sticky').children().click(function(){
            if(this.className === "portfolio"){
                window.location ='../logodesign/Portfolio.html'      
            }
            else if(this.className === "our-packages"){
                location = `../logodesign/packages.html`
            }
            else if(this.className === "how-it-works")
            {
                window.location ='../logodesign/howitworks.html'      
    
            }
        })
    }
    else if(this.id === "webdesign2")
    {
        $(".nav-item-expand-sticky").css("left","36%")
        $('.nav-item-expand-sticky').children().click(function(){
            if(this.className === "portfolio"){
                window.location ='../webdesign/Portfolio.html'      
            }
            else if(this.className === "our-packages"){
                location = `../webdesign/packages.html`
            }
            else if(this.className === "how-it-works")
            {
                window.location ='../webdesign/howitworks.html'      
    
            }
        })
    }
    else if(this.id === "webdev2"){
        $(".nav-item-expand-sticky").css("left","49%")
        $('.nav-item-expand-sticky').children().click(function(){
            if(this.className === "portfolio"){
                window.location ='../webdevelopment/portfolio.html'      
            }
            else if(this.className === "our-packages"){
                location = `../webdevelopment/packages.html`
            }
            else if(this.className === "how-it-works")
            {
                window.location ='../webdevelopment/howitworks.html'      
    
            }
        })
    }
    $(".nav-item-expand-sticky").css("display","grid")
        $(".nav-item-expand-sticky").animate({opacity:'1'},500)
       
    })
    $(".nav-item-expand-sticky").mouseleave(function(){
        // $(this).animate({opacity:'0'},300) 
       $(this).css("display","none")
    })

    $(".sticky-items").children().click(function(){
        if(this.id === "logo2")
        {
            window.location = "/"
        }
        else if(this.id === "webdesign2")
        {
            window.location = "../webdesign/webdesign.html"
        }
        else if(this.id === "webdev2")
        {
        location = "../webdevelopment/webdev.html"
        }

    })
   

    
// console.log(height_scroll)
 
//  loaderoff
 
})
///Onstep
var stepper = ()=>{
    $(".on").css("background","rgb(8, 134, 184)")
    
}
stepper()

