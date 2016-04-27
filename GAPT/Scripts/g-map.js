// This code tells the browser to execute the "Initialize" method only when the complete document model has been loaded.
$(document).ready(function () {
    Initialize();
});

// Where all the fun happens 
function Initialize() {

    // Google has tweaked their interface somewhat - this tells the api to use that new UI
    google.maps.visualRefresh = true;
    var Liverpool = new google.maps.LatLng(53.408841, -2.981397);

    var Malta = new google.maps.LatLng(35.895780, 14.508375);

    // These are options that set initial zoom level, where the map is centered globally to start, and the type of map to show
    var mapOptions = {
        zoom: 14,
        center: Malta,
        mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    };

    // This makes the div with id "map" a google map
    var map = new google.maps.Map(document.getElementById("map"), mapOptions);

    // This shows adding a simple pin "marker" - this happens to be the Tate Gallery in Liverpool!
    var maltaLatLng = new google.maps.LatLng(53.40091, -2.994464);
    var upperBarrakkaGardens = new google.maps.LatLng(35.894699, 14.512187);

    var marker = new google.maps.Marker({
        position: upperBarrakkaGardens,
        map: map,
        title: 'Upper Barrakka Gardens'
    });

    marker.setMap(map);

    // You can make markers different colors...  google it up!
    marker.setIcon('http://maps.google.com/mapfiles/ms/icons/red-dot.png')

    // a sample list of JSON encoded data of places to visit in Liverpool, UK
    // you can either make up a JSON list server side, or call it from a controller using JSONResult
    var data = [
              { "Id": 1, "PlaceName": "Liverpool Museum", "OpeningHours": "9-5, M-F", "GeoLong": "53.410146", "GeoLat": "-2.979919" },
              { "Id": 2, "PlaceName": "Merseyside Maritime Museum ", "OpeningHours": "9-1,2-5, M-F", "GeoLong": "53.401217", "GeoLat": "-2.993052" },
              { "Id": 3, "PlaceName": "Walker Art Gallery", "OpeningHours": "9-7, M-F", "GeoLong": "53.409839", "GeoLat": "-2.979447" },
              { "Id": 4, "PlaceName": "National Conservation Centre", "OpeningHours": "10-6, M-F", "GeoLong": "53.407511", "GeoLat": "-2.984683" }
    ];

    // Using the JQuery "each" selector to iterate through the JSON list and drop marker pins
    $.each(data, function (i, item) {
        var marker = new google.maps.Marker({
            'position': new google.maps.LatLng(item.GeoLong, item.GeoLat),
            'map': map,
            'title': item.PlaceName
        });

        // Make the marker-pin blue!
        marker.setIcon('http://maps.google.com/mapfiles/ms/icons/blue-dot.png')

        // put in some information about each json object - in this case, the opening hours.
        var infowindow = new google.maps.InfoWindow({
            content: "<div class='infoDiv'><h2>" + item.PlaceName + "</h2>" + "<div><h4>Opening hours: " + item.OpeningHours + "</h4></div></div>"
        });

        // finally hook up an "OnClick" listener to the map so it pops up out info-window when the marker-pin is clicked!
        google.maps.event.addListener(marker, 'click', function () {
            infowindow.open(map, marker);
        });

    })
}