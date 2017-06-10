jQuery(function($) {
  var latitude = $('#google-map').data('latitude')
  var longitude = $('#google-map').data('longitude')

  function initialize_map() {
      google.maps.visualRefresh = true; 
    var myLatlng = new google.maps.LatLng(latitude, longitude);
    var mapOptions = {
      zoom: 8,
      //scrollwheel: false,
      center: myLatlng,
      mapTypeId: google.maps.MapTypeId.G_NORMAL_MAP
    };
    var map = new google.maps.Map(document.getElementById('google-map'), mapOptions);
    var contentString = '';
    var infowindow = new google.maps.InfoWindow({
       content: "<div class='infoDiv'><h2>" + "NAMEEE" + "</div></div>"
    });
    var marker = new google.maps.Marker({
      position: myLatlng,
      map: map
    });
      marker.setIcon('http://maps.google.com/mapfiles/ms/icons/green-dot.png');
    google.maps.event.addListener(marker,
      'click',
      function() {
        infowindow.open(map, marker);
      });
  }

  google.maps.event.addDomListener(window, 'load', initialize_map);
});

