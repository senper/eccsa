import "bootstrap/dist/css/bootstrap.min.css";
import "../styles/globals.css";
import "../styles/footer.css";
import "../styles/mui_data_grid.css";
import "../styles/toast.css";
import "../styles/custom_error.css";
import { ThemeProvider } from "@mui/material/styles";
import CssBaseline from "@mui/material/CssBaseline";
import { CacheProvider } from "@emotion/react";
import theme from "../config/theme";
import createEmotionCache from "../config/createEmotionCache";
import { wrapper } from "../store";
import Layout from "../Components/Layout/Layout";
import Meta from "../Components/Layout/Meta";
import { Provider } from "react-redux";
import { getTranslations } from "./api/menu";
import React from "react";
import Script from "next/script";

// Client-side cache, shared for the whole session of the user in the browser.
const clientSideEmotionCache = createEmotionCache();

function MyApp({ Component, ...rest }) {
  const { store, props } = wrapper.useWrappedStore(rest);
  const { emotionCache = clientSideEmotionCache, pageProps } = props;
  const [additionalKeywords, setAdditionalKeywords] = React.useState();
  const [pageTitle, setPageTitle] = React.useState();

  const [translations, setTranslations] = React.useState([]);
  React.useEffect(() => {
    getTranslations().then((data) => {
      setTranslations(data.data.$values);
    });
  }, []);
  return (
    <>
      <Script
        strategy="afterInteractive"
        src="https://www.googletagmanager.com/gtag/js?id=G-V871KS7C57"
      />
      <Script
        id="google-analytics"
        strategy="afterInteractive"
        dangerouslySetInnerHTML={{
          __html: `window.dataLayer = window.dataLayer || [];
function gtag(){dataLayer.push(arguments);}
gtag('js', new Date());

gtag('config', 'G-V871KS7C57');`,
        }}
      />
      <CacheProvider value={emotionCache}>
        <Meta additionalKeywords={additionalKeywords} pageTitle={pageTitle} />
        <ThemeProvider theme={theme}>
          {/* CssBaseline kickstart an elegant, consistent, and simple baseline to build upon. */}
          <CssBaseline />
            <Provider store={store}>
              <Layout translations={translations}>
                <Component translations={translations} setAdditionalKeywords={setAdditionalKeywords} setPageTitle={setPageTitle} {...pageProps}  />
              </Layout>
            </Provider>
        </ThemeProvider>
      </CacheProvider>
    </>
  );
}

export default MyApp;
