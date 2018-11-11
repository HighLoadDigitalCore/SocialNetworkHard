var myMap = null;
var myPlacemark = null;


function EditEvent() {
    var _this = this;

  
    this.init = function () {
        $('#Place').change(function () {
            var myGeocoder = ymaps.geocode($(this).val());
            myGeocoder.then(
                function (res) {
                    //debugger;
                    if (res.geoObjects.getLength()>0) {
                        var coordinates = res.geoObjects.get(0).geometry.getCoordinates();
                        myPlacemark.geometry.setCoordinates(coordinates);
                        myMap.setCenter(coordinates);
                        var record = coordinates[0] + "|" + coordinates[1];
                        $("#Coordinate").val(record);
                        
                        var nearest = res.geoObjects.get(0);
                        var name = nearest.properties._K.metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea.Locality.LocalityName; // nearest.properties.get('name');
                        $("#CityName").val(name);

                        //$("#Place").val(nearest.properties.get('text'));

                    }
                },
                function (err) {
                    // обработка ошибки
                }
            );
           
        });
        $('#Place').keypress(function (e) {
            
            if (e.which == 13) {
                var myGeocoder = ymaps.geocode($(this).val());
                myGeocoder.then(
                    function(res) {
                        //debugger;
                        if (res.geoObjects.getLength() > 0) {
                            var coordinates = res.geoObjects.get(0).geometry.getCoordinates();
                            myPlacemark.geometry.setCoordinates(coordinates);
                            myMap.setCenter(coordinates);
                            var record = coordinates[0] + "|" + coordinates[1];
                            $("#Coordinate").val(record);

                            var nearest = res.geoObjects.get(0);
                            var name = nearest.properties._K.metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea.Locality.LocalityName;//nearest.properties.get('name');
                            $("#CityName").val(name);

                            //$("#Place").val(nearest.properties.get('text'));

                        }
                    },
                    function(err) {
                        // обработка ошибки
                    }
                );
                return false;
            }
            

        });
        $(".datePicker").datepicker({
            dateFormat: "dd.mm.yy",
            monthNames: ["Январь", "Февраль", "Март", "Апрель", "Май", "Июнь", "Июль", "Август", "Сентябрь", "Октябрь", "Ноябрь", "Декабрь"]
        });

        $("#ChangeImage").fineUploader({
            element: $('#ChangeImage'),
            request: {
                endpoint: "/File/UploadFile"
            },
            template: 'qq-template-bootstrap',
            allowedExtensions: ['jpg', 'jpeg', 'png', 'gif'],
            classes: {
                success: 'alert alert-success',
                fail: 'alert alert-error'
            },
            failedUploadTextDisplay: {
                mode: 'custom',
                maxChars: 400,
                responseProperty: 'error',
                enableTooltip: true
            },
            debug: true
        })
       .on('complete', function (event, id, filename, responseJSON) {
           if (responseJSON.success) {
               $("#ImagePath").val(responseJSON.fileUrl);
               $("#Image").show();
               $("#ChangeImage").addClass("opacity");
               $("#Image").attr("src", responseJSON.fileUrl + "?w=156&h=168&mode=crop");
           }
       }).on('submit', function () {
           $(".qq-upload-fail").remove();
           return true;
       });

        if($("#ImagePath").val().length > 0)
        {
            $("#Image").show();
            $("#ChangeImage").addClass("opacity");
        }

    };


    this.initMap = function () {
        if ($("#Coordinate").val().length > 0) {
            var coord = $("#Coordinate").val();
            var coords = coord.split("|");
            myMap = new ymaps.Map('map', {
                // При инициализации карты обязательно нужно указать
                // её центр и коэффициент масштабирования.
                center: [coords[0], coords[1]], // Москва
                zoom: 11
            });
            myPlacemark = new ymaps.Placemark([coords[0], coords[1]], null, { draggable: true });
        } else {
          
            myMap = new ymaps.Map('map', {
                // При инициализации карты обязательно нужно указать
                // её центр и коэффициент масштабирования.
                center: [55.76, 37.64], // Москва
                zoom: 11
            });
            myPlacemark = new ymaps.Placemark([55.76, 37.64], null, { draggable: true });
        }
        myMap.controls.add('zoomControl');
        myMap.geoObjects.add(myPlacemark);
        myPlacemark.events.add('dragend', function (origEvent, sourceEvent) {
            loadInputs();
        });
        //loadInputs();
    }
   
}

function loadInputs() {
        var coordinates = myPlacemark.geometry.getCoordinates();

        var myReverseGeocoder = ymaps.geocode(coordinates, { kind: 'house' });

        myReverseGeocoder.then(
            function (res) {
                if (res.geoObjects.getLength() > 0) {
                    var nearest = res.geoObjects.get(0);
                    //var name = nearest.properties.get('name');
                    $("#CityName").val(nearest.properties._K.metaDataProperty.GeocoderMetaData.AddressDetails.Country.AdministrativeArea.SubAdministrativeArea.Locality.LocalityName);
                    $("#Place").val(nearest.properties.get('text'));
                } else {
                    myReverseGeocoder = ymaps.geocode(coordinates, { kind: 'locality' });
                    myReverseGeocoder.then(
                        function(res) {
                            if (res.geoObjects.getLength() > 0) {
                                var nearest = res.geoObjects.get(0);
                                var name = nearest.properties.get('name');
                                $("#CityName").val(name);
                                $("#Place").val(nearest.properties.get('text'));
                            }
                        });
                }

            },
            function (err) {
                alert('Ошибка');
            }
        );
        var record = coordinates[0] + "|" + coordinates[1];
        $("#Coordinate").val(record);
}

function initMap()
{
    editEvent.initMap()
}
var editEvent = null;
$().ready(function ()
{
    editEvent = new EditEvent();
    editEvent.init();
});

ymaps.ready(initMap);