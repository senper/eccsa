import React from "react";
import MainMenu from "./MainMenu";
import styles from "../../styles/Layout.module.css";
import BottomAdvert from "./BottomAdvert";
import TopAdvert from "./TopAdvert";
import Link from "next/link";
import { LoginMenu } from "../Login/LoginMenu";
import { Grid } from "@mui/material";
import Footer from "../Footer/Footer";
import { Router, useRouter } from "next/router";
import { loadUserFromStorage } from "../../api/oauth";
import { useSelector } from "react-redux";
import { wrapper } from "../../store";
import { getLocalByKey } from "../../pages/api/menu";
import SurveyPopup from "../Survey/SurveyPopup";

const Layout = ({ children, translations, ...rest }) => {
  ///
  const { store } = wrapper.useWrappedStore(rest);
  const tokenMain =
    typeof window !== "undefined" ? localStorage.getItem("access_token") : null;

  const [userLoading, setUserLoading] = React.useState(true);

  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );
  React.useEffect(() => {
    if (!user) {
      setUserLoading(true);
      loadUserFromStorage(store).then((user) => {
        setUser(user ? user.data : null);
        setUserLoading(false);
      });
    }
  }, [tokenMain]);
  ///
  const router = useRouter();
  const { locale } = router;
  function changeLang(e) {
    e.preventDefault();
    const { pathname, asPath, query } = router;
    // change just the locale and maintain all other route information including href's query
    router.push({ pathname, query }, asPath, {
      locale: locale == "en" ? "am" : "en",
    });
  }
  //
  const [loading, setLoading] = React.useState(false);
  React.useEffect(() => {
    const start = () => {
      setLoading(true);
    };
    const end = () => {
      setLoading(false);
    };
    Router.events.on("routeChangeStart", start);
    Router.events.on("routeChangeComplete", end);
    Router.events.on("routeChangeError", end);
    return () => {
      Router.events.off("routeChangeStart", start);
      Router.events.off("routeChangeComplete", end);
      Router.events.off("routeChangeError", end);
    };
  }, []);
  ///
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  ///
  return <>
    <div className={styles.fixedTop} style={{ backgroundColor: "#fff" }}>
      <Grid container spacing={1} className={styles.nav}>
        <Grid item sm={5}>
          <Link href="/" legacyBehavior>
            <img
              src="/images/header.jpg"
              width={"100%"}
              alt="eccsa header logo"
            />
          </Link>
        </Grid>
        <Grid item sm={5}>
          <TopAdvert />
        </Grid>
        <Grid item sm={2} className={styles.linkAndLang}>
          <Link
            href={"/"}
            onClick={(e) => changeLang(e)}
            style={{ float: "right", paddingRight: "28px" }}>

            {locale == "en" ? "አማ" : "En"}

          </Link>
          <br />
          <LoginMenu
            store={store}
            setUser={setUser}
            user={user}
            getLocal={getLocal}
          />
        </Grid>
      </Grid>
      <MainMenu user={user} />
    </div>
    <div style={{ marginTop: "155px" }} className="d-block d-sm-none"></div>
    <div
      style={{ marginTop: user ? "2px" : "15px" }}
      className="d-none d-lg-block"
    ></div>
    <br />
    <div style={{ marginTop: user ? "146px" : "120px" }} className=""></div>
    <div>
      {/* {userLoading ? null : loading ? (
        <h3 style={{ fontWeight: "bolder" }}>{getLocal("Loading")}...</h3>
      ) : (
        children
      )} */}
      {userLoading ? null : loading ? (
        <h3></h3>
      ) : (
        React.Children.map(children, (child) => {
          // Checking isValidElement is the safe way and avoids a
          // typescript error too.
          if (React.isValidElement(child)) {
            return React.cloneElement(child, { user, setUser });
          }
          return child;
        })
      )}
    </div>
    <Footer getLocal={getLocal} getLocalByVal={getLocalByVal} />
    <div
      style={{ textAlign: "right", marginTop: "-50px", zIndex: "-1" }}
      className={styles.marginBottom}
    >
      Powered By{" "}
      <a href="http://idfeth.com/" target={"_blank"} rel="noreferrer">
        <img
          style={{
            width: "100%",
            height: "100%",
            maxHeight: "35px",
            maxWidth: "35px",
          }}
          src="/images/idflogo.png"
          alt="iDF plc"
        />
      </a>
    </div>
    {/* <div className={styles.fixedBottom}> 
      <BottomAdvert />
    </div>*/}

    <div className={styles.fixedBottomLeft}>
      <SurveyPopup user={user} getLocal={getLocal} />
    </div>
  </>;
};

export default Layout;
