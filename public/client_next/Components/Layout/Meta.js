import Head from "next/head";
const Meta = ({
  title,
  keywords,
  description,
  additionalKeywords,
  pageTitle,
}) => {
  keywords += `, ${additionalKeywords.join(", ")}`;
  //! Note sure which one is correct
  return (
    <Head>
      <meta name="application-name" content="ECDS" />

      <meta name="keywords" content={keywords} />
      <meta name="description" content={description} />
      <meta name="og:description" content={keywords} />

      <meta name="apple-mobile-web-app-capable" content="yes" />
      <meta name="apple-mobile-web-app-status-bar-style" content="default" />
      <meta name="apple-mobile-web-app-title" content="ECDS" />
      <meta name="format-detection" content="telephone=no" />
      <meta name="mobile-web-app-capable" content="yes" />
      <meta name="msapplication-config" content="/icons/browserconfig.xml" />
      <meta name="msapplication-TileColor" content="#198754" />
      <meta name="msapplication-tap-highlight" content="no" />
      <meta name="theme-color" content="#b6e6f4" />

      <meta name="twitter:card" content="summary" />
      <meta name="twitter:url" content="https://twitter.com/EthiopiaChamber" />
      <meta name="twitter:title" content="Ethiopian Chamber" />
      <meta name="twitter:description" content="Ethiopian Chamber" />
      {/* <meta name="twitter:image" content="https://yourdomain.com/icons/android-chrome-192x192.png" />
<meta name="twitter:creator" content="@DavidWShadow" /> */}
      <meta property="og:type" content="website" />
      <meta
        property="og:title"
        content="Ethiopian Chamber of Commerce and Sectoral Associations Digital Service (ECDS)"
      />
      <meta property="og:site_name" content="ECDS" />
      <meta property="og:url" content="https://ethiopianchamber.et/" />
      <meta
        property="og:image"
        content="https://ethiopianchamber.et/images/logo.png"
      />

      <meta charSet="utf-8" />
      <meta
        name="viewport"
        content="minimum-scale=1, initial-scale=1, width=device-width, shrink-to-fit=no, viewport-fit=cover"
      />

      <title>{`${pageTitle && pageTitle.length > 0 ? pageTitle + ": " : ""}${title}`}</title>
    </Head>
  );
};

Meta.defaultProps = {
  title:
    "Ethiopian Chamber of Commerce and Sectoral Associations Digital Service (ECDS)",
  keywords:
    "Ethiopian, Chamber, Commerce, Sectorial, Sectoral, Sector, Associations, Business, Trade, Industry, Economy, match, making, trade, facilitation, investment, certificate, origin, commercial, invoice, arbitration, academy, promotion, membership, issue, events, advert, advertise, advertisement",
  description:
    "Ethiopian, Chamber, Commerce, Sectorial, Sectoral, Sector, Associations, Business, Trade, Industry, Economy, match, making, trade, facilitation, investment, certificate, origin, commercial, invoice, arbitration, academy, promotion, membership, issue, events, advert, advertise, advertisement",
  additionalKeywords: [],
  pageTitle: "",
};

export default Meta;
