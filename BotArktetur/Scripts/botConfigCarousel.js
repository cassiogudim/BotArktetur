var idItemConversation;
var heightItemCarousel;
var heightItemContentTextCarousel;
var countItensCarousel;
var countItensLoop;

var scrollPage = 50;

$(document).ready(function () {
    //$('body').on('DOMNodeInserted', '.wc-message-wrapper', function (e) {
    //    // verificar item pai aqui

    //    // fazer scrooltop
    //    //var testeItem = $("div").find("[data-activity-id*='000']");
    //    //scrollPage = 0;
    //    //$($(testeItem)).each(function (index) {
    //    //    scrollPage += $(this).height();
    //    //    $(".wc-message-groups").animate({ scrollTop: scrollPage }, 1000);
    //    //});
    //});

    $('body').on('DOMNodeInserted', '.wc-carousel-item', function (e) {
        var idItemPai = $(e.target).parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().parent().attr('data-activity-id');
        //verificar se é o primeiro item
        if (idItemPai != null || idItemPai != "" || idItemPai != undefined) {
            if (idItemConversation != idItemPai) {
                idItemConversation = idItemPai;
                heightItemCarousel = 0;
                heightItemContentTextCarousel = 0;
                countItensCarousel = 0;
                countItensLoop = 0;
            }
        }

        // carrossel com imagem, titulo e subtitulo
        if ($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children().length > 3) {
            heightItemCarrosselImageTitle = $($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[4]).height();
            $($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[2]).height(18);
            $($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[4]).height(heightItemCarrosselImageTitle + 30);

            $($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[0]).css('min-height', '120px');

            $($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[0]).children().css('width', 'auto');
            $($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[0]).children().css('margin', 'auto');
            //$($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().height(heightItemCarousel);
            //$($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[countItensLoop]).children().children().children().children().children()[5]).css('top', '0px');
        }

        configureCarouselHeightItens(e);
        
        countItensLoop++;

        if (countItensLoop == countItensCarousel) {
            //debugger;
            setHeightToCarousel();
            if ($($("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]')[0]).children().children().children().children().children().length > 3) {
                //$("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]').children().children().height(heightItemCarousel);
            }
            setTimeout(function () {
                //readjust image
                readjustCardHero();
            }, 2000);
        }

        $("div[data-activity-id='" + idItemConversation + "']").find('div[class="wc-message-content"]')
            .css('border', '1px transparent solid');
        $("div[data-activity-id='" + idItemConversation + "']").find('div[class="wc-message-content"]')
            .css('background-color', 'transparent');
        $("div[data-activity-id='" + idItemConversation + "']").find('div[class="wc-message-content"]')
            .css('padding', '0px');
        $("div[data-activity-id='" + idItemConversation + "']").find('div[class="wc-message-content"]')
            .css('box-shadow', '0px 0px 0px 0px rgba(0, 0, 0, 0.2)');
        $("div[data-activity-id='" + idItemConversation + "']").find('svg[class="wc-message-callout"]').remove();
    });
});

function configureCarouselHeightItens(item) {
    if ($(item.currentTarget).height() > heightItemCarousel) {
        heightItemCarousel = parseInt($(item.currentTarget).height() - 2);
    }

    //setar tamanho para os itens de carousel da lista
    var itensCarousel = $("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]');
    //pegando numero de itens no carousel
    countItensCarousel = $("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]').length;

    $($(itensCarousel)).each(function (index) {
        var element = this;
        var classChildren = $(this).children().attr('class')
        //card hero
        if (classChildren == "wc-card wc-adaptive-card hero") {
            if ($(this).find('div[class="ac-image"]')) {
                //pegar tamanho ao carregar a imagem para reajustar box
                $('img').each(function () {
                    if (this.complete) {
                        imageLoadedCarouselHeroCard.call(element);
                    }
                    else {
                        $(this).one('load', imageLoadedCarouselHeroCard(element));
                    }
                });

                var heightTotal = $(this).find('div[class="ac-image"]').height();
                if (heightTotal > heightItemCarousel) {
                    heightItemCarousel = $(this).find('div[class="ac-image"]').height();
                }
            }
            else {
                if ($(this).height() > heightItemCarousel) {
                    heightItemCarousel = $(this).height();
                }
            }

            //pegar tamanho do maior texto para setar altura nos outros itens para alinhamento do botão
            var textContentHeight = $(this).children().children().children().children().height();
            if (textContentHeight > heightItemContentTextCarousel)
                heightItemContentTextCarousel = textContentHeight;
        }
        //thumbnail card
        else if (classChildren = "wc-card wc-adaptive-card thumbnail") {
            //if ($(this).find('div[class="ac-image"]')) {
            //    //pegar tamanho ao carregar a imagem para reajustar box
            //    $('img').each(function () {
            //        if (this.complete) {
            //            imageLoadedCarouselHeroCard.call(element);
            //        }
            //        else {
            //            $(this).one('load', imageLoadedCarouselHeroCard(element));
            //        }
            //    });

            //    var heightTotal = $(this).find('div[class="ac-image"]').height();
            //    if (heightTotal > heightItemCarousel) {
            //        heightItemCarousel = $(this).find('div[class="ac-image"]').height();
            //    }
            //}
            //else {
            if ($(this).height() > heightItemCarousel) {
                heightItemCarousel = $(this).height();
            }
            //}

            //pegar tamanho do maior texto para setar altura nos outros itens para alinhamento do botão
            var textContentHeight = $(this).children().children().children().children().height();
            if (textContentHeight > heightItemContentTextCarousel)
                heightItemContentTextCarousel = textContentHeight;
        }
        else {
            if ($(this).height() > heightItemCarousel) {
                heightItemCarousel = $(this).height();
            }

            //pegar tamanho do maior texto para setar altura nos outros itens para alinhamento do botão
            var textContentHeight = $(this).children().children().children().children().height();
            if (textContentHeight > heightItemContentTextCarousel)
                heightItemContentTextCarousel = textContentHeight;
        }
    });
}

// function to invoke for loaded image
function imageLoadedCarouselHeroCard(element) {
    if (element) {
        var heightTotal = parseInt($(element).height() + $("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]').find('div[class="ac-image"]').find('img').height());
        if (heightTotal > heightItemCarousel)
            heightItemCarousel = heightTotal;
    }
}

function setHeightToCarousel() {
    var itensCarousel = $("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]');

    $($(itensCarousel)).each(function (index) {
        //setando altura para o box carousel item
        var classChildren = $(this).children().attr('class');
        //if (classChildren == "wc-card wc-adaptive-card thumbnail") {
        //    $(this).children().height(heightItemCarousel);
        //    $(this).find('div[class="ac-columnSet"]').height(heightItemContentTextCarousel);
        //}
    });
}

//reajustando tamanho do card hero
function readjustCardHero() {
    var ehThumbnail = false;
    var itensCarousel = $("div[data-activity-id='" + idItemConversation + "']").find('li[class="wc-carousel-item"]');

    $($(itensCarousel)).each(function (index) {
        //setando altura para o box carousel item
        var classChildren = $(this).children().attr('class');
        if (classChildren == "wc-card wc-adaptive-card hero") {
            if ($(this).height() > heightItemCarousel) {
                heightItemCarousel = $(this).height();
            }
        }
        else if (classChildren == "wc-card wc-adaptive-card thumbnail") {
            ehThumbnail = true;
            if ($(this).height() > heightItemCarousel) {
                heightItemCarousel = $(this).height();
            }
        }
    });

    if (ehThumbnail) {
        $($(itensCarousel)).each(function (index) {
            $(this).children().children().height(heightItemCarousel);
        });
    }
    else {
        $($(itensCarousel)).each(function (index) {
            $(this).children().height(heightItemCarousel);
        });
    }
}