(function ($) {

	new WOW().init();

	jQuery(window).load(function() { 
		jQuery("#preloader").delay(100).fadeOut("slow");
		jQuery("#load").delay(100).fadeOut("slow");
	});


	//jQuery to collapse the navbar on scroll
	$(window).scroll(function() {
		if ($(".navbar").offset().top > 50) {
			$(".navbar-fixed-top").addClass("top-nav-collapse");
		} else {
			$(".navbar-fixed-top").removeClass("top-nav-collapse");
		}
	});

	//jQuery for page scrolling feature - requires jQuery Easing plugin
	$(function() {
		$('.navbar-nav li a').bind('click', function(event) {
			var $anchor = $(this);
			$('html, body').stop().animate({
				scrollTop: $($anchor.attr('href')).offset().top
			}, 1500, 'easeInOutExpo');
			event.preventDefault();
		});
        $('.page-scroll a').bind('click', function (event) {
            alert("Hello World");
			var $anchor = $(this);
			$('html, body').stop().animate({
				scrollTop: $($anchor.attr('href')).offset().top
			}, 1500, 'easeInOutExpo');
            event.preventDefault();
            alert("Hello World");
            console.log();
            $.ajax({
                url: 'http://localhost:52240/api/departments/',
                method: 'GET',
                success: function (officeList) {
                    //$('#msg').html(officeList.length + " departments found");
                    console.log(officeList);
                    officeList.forEach(function (office) {
                        alert(office.officeId);
                        $('#division').html(office.OfficeId);
                        var row = "<oi><a href='#'>" + office.location + "</a></oi>";
                        $('#officeList').append(row);
                    });

                }
            });
		});
	});

})(jQuery);
