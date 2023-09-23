namespace TrackingAmazonPrices.Tests.Infraestructure.Unit.Mocks;

public static class MockHtmlContentAmazon
{
    public static string GetHtmlContent()
    {
        return
            """
               <!doctype html><html class="a-no-js" data-19ax5a9jf="dingo">
                <head>
            <script type="text/javascript">var ue_t0=ue_t0||+new Date();</script>
            <script type="text/javascript">

            // some super large HTML 

            <link rel="canonical" href="https://www.amazon.es/programador-pragm%C3%A1tico-Edici%C3%B3n-especial-ESPECIALES/dp/8441545871" />
            <meta name="description" content="El programador pragmático. Edición especial: Viaje a la maestría (TÍTULOS ESPECIALES) : Thomas, David, Hunt, Andrew: Amazon.es: Libros" />
            <meta name="title" content="El programador pragmático. Edición especial: Viaje a la maestría (TÍTULOS ESPECIALES) : Thomas, David, Hunt, Andrew: Amazon.es: Libros" />
            <title>El programador pragmático. Edición especial: Viaje a la maestría (TÍTULOS ESPECIALES) : Thomas, David, Hunt, Andrew: Amazon.es: Libros</title>
            
            <div id="nav-shop">
             </div>
                    <div id='nav-xshop-container'>
                      <div id='nav-xshop' class="nav-progressive-content">
                        <script type='text/javascript'>window.navmet.tmp=+new Date();</script>
            <a href="/hz/contact-us/accessibility" aria-label="Haz clic para contactar con el Soporte de Accesibilidad o llámanos directamente al 900-83-17-74." class="nav-hidden-aria  " tabindex="0"  data-csa-c-type="link" data-csa-c-slot-id="nav_cs_0" >Servicio al cliente con discapacidad</a>
            
            <a href="/gp/bestsellers/?ref_=nav_cs_bestsellers" class="nav-a  " tabindex="0" data-csa-c-type="link" data-csa-c-slot-id="nav_cs_1" data-csa-c-content-id="nav_cs_bestsellers">Los más vendidos</a>
            
                        </div>
                                          <div id="twisterPlusWWDesktop" class="celwidget" data-feature-name="twisterPlusWWDesktop"
                             data-csa-c-type="widget" data-csa-c-content-id="twisterPlusWWDesktop"
                             data-csa-c-slot-id="twisterPlusWWDesktop" data-csa-c-asin="8441545871"
                             data-csa-c-is-in-initial-active-row="false">
                                                                                                                                                <div class="a-section aok-hidden twister-plus-buying-options-price-data">{"desktop_buybox_group_1":[{"displayPrice":"37,95 €","priceAmount":37.95,"currencySymbol":"€","integerValue":"37","decimalSeparator":",","fractionalValue":"95","symbolPosition":"right","hasSpace":true,"showFractionalPartIfEmpty":true,"offerListingId":"NyCTDZgAjodrNfmUY9Zza4LN6ncsEoVwlsFw5Psg27H3MxO8fsv2NBzoDz%2FNac%2B418yLDLgk%2F3cgjCWdUj73bniKKVvE%2B01mgIwwHF82vadBRowaElZRmMNruNypxwRvLhjVGpUCsNa399JSbRxfRQ%3D%3D","locale":"es-ES","buyingOptionType":"NEW","aapiBuyingOptionIndex":0}]}</div>         <div id="twister-plus-feature" class="a-section a-spacing-large aok-hidden"> <h3 class="a-spacing-small a-spacing-top-large twister-plus-header"> Opciones de compra y complementos </h3> <hr aria-hidden="true" class="a-spacing-small a-divider-normal twister-plus-divider"/>                              <div id="tp-cc-cards-refresh-strings" data-multiple-card-invalid-or-selection-invalid-message="Las mejoras elegidas no están disponibles para este vendedor" data-multiple-details-update-or-cards-refresh-message="Las mejoras elegidas se han actualizado" data-single-card-invalid-message="###cardName no está disponible para esta opción" data-single-details-update-message="###cardName que elegiste se ha actualizado" data-single-selection-invalid-message="###cardName elegido no está disponible para esta opción" class="a-section aok-hidden"> </div> <div id="tp-cc-cards-refresh-red-notification" class="a-section aok-hidden"> <div class="a-box a-alert-inline a-alert-inline-error" role="alert"><div class="a-box-inner a-alert-container"><i class="a-icon a-icon-alert"></i><div class="a-alert-content"></div></div></div> </div> <div id="tp-cc-cards-refresh-green-notification" class="a-section aok-hidden"> <div class="a-box a-alert-inline a-alert-inline-info" aria-live="polite" aria-atomic="true"><div class="a-box-inner a-alert-container"><i class="a-icon a-icon-alert"></i><div class="a-alert-content"></div></div></div> </div> <input type="hidden" id="twister-plus-active-cards" value="" />     <input type="checkbox" id ="twister-plus-checkbox" class= "aok-hidden" />
                </div>       <input type="hidden" id="twister-plus-device-type" value="web" />
            <input type="hidden" id="twister-plus-eligible" value="true" />
            <input type="hidden" id="ccCardsRendered" value="false" />
            <input type="hidden" id="twister-plus-asin" value="8441545871" />
            
            // more super large HTML 

            """;
    }

    public static string GetHtmlContentWithoutDescription()
    {
        return
            """
               <!doctype html><html class="a-no-js" data-19ax5a9jf="dingo">
                <head>
            <script type="text/javascript">var ue_t0=ue_t0||+new Date();</script>
            <script type="text/javascript">

            // some super large HTML 

            
            <div id="nav-shop">
             </div>
                    <div id='nav-xshop-container'>
                      <div id='nav-xshop' class="nav-progressive-content">
                        <script type='text/javascript'>window.navmet.tmp=+new Date();</script>
            <a href="/hz/contact-us/accessibility" aria-label="Haz clic para contactar con el Soporte de Accesibilidad o llámanos directamente al 900-83-17-74." class="nav-hidden-aria  " tabindex="0"  data-csa-c-type="link" data-csa-c-slot-id="nav_cs_0" >Servicio al cliente con discapacidad</a>
            
            <a href="/gp/bestsellers/?ref_=nav_cs_bestsellers" class="nav-a  " tabindex="0" data-csa-c-type="link" data-csa-c-slot-id="nav_cs_1" data-csa-c-content-id="nav_cs_bestsellers">Los más vendidos</a>
            
                        </div>
                                          <div id="twisterPlusWWDesktop" class="celwidget" data-feature-name="twisterPlusWWDesktop"
                             data-csa-c-type="widget" data-csa-c-content-id="twisterPlusWWDesktop"
                             data-csa-c-slot-id="twisterPlusWWDesktop" data-csa-c-asin="8441545871"
                             data-csa-c-is-in-initial-active-row="false">
                                                                                                                                                <div class="a-section aok-hidden twister-plus-buying-options-price-data">{"desktop_buybox_group_1":[{"displayPrice":"37,95 €","priceAmount":37.95,"currencySymbol":"€","integerValue":"37","decimalSeparator":",","fractionalValue":"95","symbolPosition":"right","hasSpace":true,"showFractionalPartIfEmpty":true,"offerListingId":"NyCTDZgAjodrNfmUY9Zza4LN6ncsEoVwlsFw5Psg27H3MxO8fsv2NBzoDz%2FNac%2B418yLDLgk%2F3cgjCWdUj73bniKKVvE%2B01mgIwwHF82vadBRowaElZRmMNruNypxwRvLhjVGpUCsNa399JSbRxfRQ%3D%3D","locale":"es-ES","buyingOptionType":"NEW","aapiBuyingOptionIndex":0}]}</div>         <div id="twister-plus-feature" class="a-section a-spacing-large aok-hidden"> <h3 class="a-spacing-small a-spacing-top-large twister-plus-header"> Opciones de compra y complementos </h3> <hr aria-hidden="true" class="a-spacing-small a-divider-normal twister-plus-divider"/>                              <div id="tp-cc-cards-refresh-strings" data-multiple-card-invalid-or-selection-invalid-message="Las mejoras elegidas no están disponibles para este vendedor" data-multiple-details-update-or-cards-refresh-message="Las mejoras elegidas se han actualizado" data-single-card-invalid-message="###cardName no está disponible para esta opción" data-single-details-update-message="###cardName que elegiste se ha actualizado" data-single-selection-invalid-message="###cardName elegido no está disponible para esta opción" class="a-section aok-hidden"> </div> <div id="tp-cc-cards-refresh-red-notification" class="a-section aok-hidden"> <div class="a-box a-alert-inline a-alert-inline-error" role="alert"><div class="a-box-inner a-alert-container"><i class="a-icon a-icon-alert"></i><div class="a-alert-content"></div></div></div> </div> <div id="tp-cc-cards-refresh-green-notification" class="a-section aok-hidden"> <div class="a-box a-alert-inline a-alert-inline-info" aria-live="polite" aria-atomic="true"><div class="a-box-inner a-alert-container"><i class="a-icon a-icon-alert"></i><div class="a-alert-content"></div></div></div> </div> <input type="hidden" id="twister-plus-active-cards" value="" />     <input type="checkbox" id ="twister-plus-checkbox" class= "aok-hidden" />
                </div>       <input type="hidden" id="twister-plus-device-type" value="web" />
            <input type="hidden" id="twister-plus-eligible" value="true" />
            <input type="hidden" id="ccCardsRendered" value="false" />
            <input type="hidden" id="twister-plus-asin" value="8441545871" />
            
            // more super large HTML 

            """;
    }
}
