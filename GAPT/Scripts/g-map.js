// This code tells the browser to execute the "Initialize" method only when the complete document model has been loaded.
$(document).ready(function () {
    Initialize();
});

// Where all the fun happens 
function Initialize() {

    // Google has tweaked their interface somewhat - this tells the api to use that new UI
    google.maps.visualRefresh = true;
    var Liverpool = new google.maps.LatLng(53.408841, -2.981397);

    var Malta = new google.maps.LatLng(35.892586, 14.448172);

    // These are options that set initial zoom level, where the map is centered globally to start, and the type of map to show
    var mapOptions = {
        zoom: 12,
        center: Malta,
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    };

    // This makes the div with id "map" a google map
    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    $.ajax({
        url: "/Home/GetJsonLocations",
        type: "POST",
        async: false,
        success: function (jsonData) {
            var data = $.map(jsonData, function (el) { return el });
            $.each(data, function (i, item) {
                var marker = new google.maps.Marker({
                    'position': new google.maps.LatLng(item.Latitude, item.Longitude),
                    'map': map,
                    'title': item.Name
                });

                // Make the marker-pin blue!
                marker.setIcon('http://maps.google.com/mapfiles/ms/icons/red-dot.png')

                // put in some information about each json object - in this case, the opening hours.
                var infowindow = new google.maps.InfoWindow({
                    content: "<div class='infoDiv'><h2>" + item.Name + "</h2>" + "</div>"
                });

                // finally hook up an "OnClick" listener to the map so it pops up out info-window when the marker-pin is clicked!
                google.maps.event.addListener(marker, 'click', function () {
                    infowindow.open(map, marker);
                });

            })
        },
        error: function () {
            alert("error");
        }
    });
}