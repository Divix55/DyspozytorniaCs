function load() {
    var map = new google.maps.Map(document.getElementById("map"), {
        center: new google.maps.LatLng(54.371765, 18.611966),
        zoom: 13,
        mapTypeId: 'roadmap'
    });
    var info = new google.maps.InfoWindow;

    function dodajMarker(lat, lng, txt) {
        var opcjeMarkera =
            {
                position: new google.maps.LatLng(lat, lng),
                map: map
            }
        var marker = new google.maps.Marker(opcjeMarkera);
        marker.txt = txt;

        google.maps.event.addListener(marker, "click", function () {
            info.setContent(marker.txt);
            info.open(map, marker);
        });
        return marker;
    }

    $(function () {
        var n = document.getElementsByClassName('name');
        var x = document.getElementsByClassName('lon');
        var y = document.getElementsByClassName('lat');
        var i = 0;
        $('.wiersz').each(function(){
            var marker = dodajMarker(x[i].innerHTML, y[i].innerHTML, n[i].innerHTML);
            i++;
        });
    });
}

function initMap(){
    var directionsService = new google.maps.DirectionsService;
    var directionsDisplay = new google.maps.DirectionsRenderer;
    var map = new google.maps.Map(document.getElementById('map'), {
        center: {lat: 54.371765, lng: 18.611966},
        zoom: 13,
        mapTypeId: 'roadmap'
    });
    directionsDisplay.setMap(map);
    var onChangeHandler = function() {
        calculateAndDisplayRoute(directionsService, directionsDisplay);
    };
    document.getElementById("myBtn").addEventListener("click", onChangeHandler);
}

function calculateAndDisplayRoute(directionsService, directionsDisplay) {
    var rows = document.getElementById("tabela_dostawy").getElementsByTagName("tr").length;
    var nr = document.getElementById("nr_dostawy").value;

    var storelat = document.getElementById("tabela_dostawy").rows[nr].cells[4].innerHTML;
    var storelng = document.getElementById("tabela_dostawy").rows[nr].cells[5].innerHTML;
    var magazyn = new google.maps.LatLng(storelat, storelng);
    var przystanki = [];

    for(var i=nr;i<rows;i++)
    {
        var trasa = document.getElementById("tabela_dostawy").rows[i].cells[11].innerHTML;
        if(trasa == nr)
        {
            var shoplat = document.getElementById("tabela_dostawy").rows[i].cells[2].innerHTML;
            var shoplng = document.getElementById("tabela_dostawy").rows[i].cells[3].innerHTML;
            var sklep = new google.maps.LatLng(shoplat, shoplng);
            przystanki.push({location: sklep, stopover: true});
        }

    }


    directionsService.route({
        origin: magazyn,
        destination: magazyn,
        waypoints: przystanki,
        optimizeWaypoints: true,
        travelMode: 'DRIVING'
    }, function(response, status) {
        if (status === 'OK') {
            directionsDisplay.setDirections(response);
        } else {
            window.alert('Directions request failed due to ' + status);
        }
    });
}
