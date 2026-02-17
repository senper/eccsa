/* eslint-disable react/prop-types */
import React from "react";
import cx from "classnames";
import {
  Switch,
  Route,
  Redirect,
  useLocation,
  useHistory,
} from "react-router-dom";
import { useSelector } from "react-redux";
// creates a beautiful scrollbar
import PerfectScrollbar from "perfect-scrollbar";
import "perfect-scrollbar/css/perfect-scrollbar.css";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";

// core components
import AdminNavbar from "components/vender/Navbars/AdminNavbar.js";
import Footer from "components/vender/Footer/Footer.js";
import Sidebar from "components/vender/Sidebar/Sidebar.js";
import FixedPlugin from "components/vender/FixedPlugin/FixedPlugin.js";

import styles from "assets/jss/material-dashboard-pro-react/layouts/adminStyle.js";
import { createBrowserHistory } from "history";
import { USER_PROFILE_QUERY } from "variables/user_gql";
import { useQuery } from "urql";
import Snackbar from "components/vender/Snackbar/Snackbar.js";
import AddAlert from "@material-ui/icons/AddAlert";

var ps;

const useStyles = makeStyles(styles);

export default function Dashboard(props) {
  //
  const [messageProps, setMessageProps] = React.useState({
    color: "info",
    open: false,
    message: "",
  });
  React.useEffect(() => {
    if (messageProps && messageProps.open) {
      setTimeout(function () {
        setMessageProps({ ...messageProps, open: false });
      }, 6000);
    }
  }, [messageProps]);
  //
  //const user = useSelector((state) => state.auth.user);
  const location = useLocation();
  const history = useHistory();
  const [result, executeQuery] = useQuery({
    query: USER_PROFILE_QUERY,
  });
  const { data, fetching, error } = result;

  const [loginUser, setLoginUser] = React.useState(null);
  React.useEffect(() => {
    if (result.data) {
      const user = result.data.userProfile;
      //console.log(user);
      if (
        user &&
        props.moduleId &&
        (user.assignedModules & props.moduleId) != props.moduleId
      ) {
        history.push({
          pathname: "/auth/login-page",
          state: {
            error: "User don't have the appropriate role for trade promotion",
          },
        });
      } else if (user) {
        setLoginUser(user);
      }
    } else if (result.error) {
      history.push({
        pathname: "/auth/login-page",
        state: {
          error: "Unable to get user profile",
        },
      });
    }
  }, [result.data]);
  const { ...rest } = props;
  // states and functions
  const [mobileOpen, setMobileOpen] = React.useState(false);
  const [miniActive, setMiniActive] = React.useState(false);
  const [image, setImage] = React.useState(
    require("assets/img/logo_org.png").default
  );
  const [color, setColor] = React.useState("blue");
  const [bgColor, setBgColor] = React.useState("black");
  // const [hasImage, setHasImage] = React.useState(true);
  const [fixedClasses, setFixedClasses] = React.useState("dropdown");
  const [logo, setLogo] = React.useState(
    require("assets/img/logo_org.png").default
  );
  // styles
  const classes = useStyles();
  const mainPanelClasses =
    classes.mainPanel +
    " " +
    cx({
      [classes.mainPanelSidebarMini]: miniActive,
      [classes.mainPanelWithPerfectScrollbar]:
        navigator.platform.indexOf("Win") > -1,
    });
  // ref for main panel div
  const mainPanel = React.createRef();
  // effect instead of componentDidMount, componentDidUpdate and componentWillUnmount
  React.useEffect(() => {
    if (navigator.platform.indexOf("Win") > -1) {
      ps = new PerfectScrollbar(mainPanel.current, {
        suppressScrollX: true,
        suppressScrollY: false,
      });
      document.body.style.overflow = "hidden";
    }
    window.addEventListener("resize", resizeFunction);
    if (!location.state) {
      history.push({
        //pathname: props.layoutUrl,
        state: { layout: props.layoutUrl, unit: props.unit },
      });
    }
    // Specify how to clean up after this effect:
    return function cleanup() {
      if (navigator.platform.indexOf("Win") > -1) {
        ps.destroy();
      }
      window.removeEventListener("resize", resizeFunction);
    };
  });
  // functions for changeing the states from components
  const handleImageClick = (image) => {
    setImage(image);
  };
  const handleColorClick = (color) => {
    setColor(color);
  };
  /*   const handleBgColorClick = (bgColor) => {
    switch (bgColor) {
      case "white":
        setLogo(require("assets/img/logo.svg").default);
        break;
      default:
        setLogo(require("assets/img/logo-white.svg").default);
        break;
    }
    setBgColor(bgColor);
  }; */
  const handleFixedClick = () => {
    if (fixedClasses === "dropdown") {
      setFixedClasses("dropdown show");
    } else {
      setFixedClasses("dropdown");
    }
  };
  const handleDrawerToggle = () => {
    setMobileOpen(!mobileOpen);
  };
  const getRoute = () => {
    return window.location.pathname !== "/admin/full-screen-maps";
  };
  const getActiveRoute = (routes) => {
    let activeRoute = "Default Brand Text";
    for (let i = 0; i < routes.length; i++) {
      if (routes[i].collapse) {
        let collapseActiveRoute = getActiveRoute(routes[i].views);
        if (collapseActiveRoute !== activeRoute) {
          return collapseActiveRoute;
        }
      } else {
        if (
          window.location.href.indexOf(routes[i].layout + routes[i].path) !== -1
        ) {
          return routes[i].name;
        }
      }
    }
    return activeRoute;
  };
  const getRoutes = (routes) => {
    return routes.map((prop, key) => {
      if (prop.collapse) {
        return getRoutes(prop.views);
      }
      if (prop.layout === props.layoutUrl) {
        return (
          <Route
            path={prop.layout + prop.path}
            // component={prop.component}
            key={key}
            render={(props) => (
              <>
                <prop.component setMessageProps={setMessageProps} />
                <Snackbar
                  place="tc"
                  icon={AddAlert}
                  {...messageProps}
                  closeNotification={() =>
                    setMessageProps({ ...messageProps }, { open: false })
                  }
                  close
                />
              </>
            )}
          />
        );
      } else {
        return null;
      }
    });
  };
  const sidebarMinimize = () => {
    setMiniActive(!miniActive);
  };
  const resizeFunction = () => {
    if (window.innerWidth >= 960) {
      setMobileOpen(false);
    }
  };

  return (
    <div className={classes.wrapper}>
      <Sidebar
        routes={props.routes}
        logoText={props.moduleName}
        logo={logo}
        image={image}
        handleDrawerToggle={handleDrawerToggle}
        open={mobileOpen}
        color={color}
        bgColor={props.bgColor}
        miniActive={miniActive}
        loginUser={loginUser}
        {...rest}
      />
      <div className={mainPanelClasses} ref={mainPanel}>
        <AdminNavbar
          sidebarMinimize={sidebarMinimize.bind(this)}
          miniActive={miniActive}
          brandText={getActiveRoute(props.routes)}
          handleDrawerToggle={handleDrawerToggle}
          loginUser={loginUser}
          {...rest}
        />
        {/* On the /maps/full-screen-maps route we want the map to be on full screen - this is not possible if the content and conatiner classes are present because they have some paddings which would make the map smaller */}
        {getRoute() ? (
          <div className={classes.content}>
            <div className={classes.container}>
              <Switch>
                {getRoutes(props.routes)}
                <Redirect from={props.layoutUrl} to={props.defaultUrl} />
              </Switch>
            </div>
          </div>
        ) : (
          <div className={classes.map}>
            <Switch>
              {getRoutes(props.routes)}
              <Redirect from={props.layoutUrl} to={props.defaultUrl} />
            </Switch>
          </div>
        )}
        {getRoute() ? <Footer fluid /> : null}
        {/* <FixedPlugin
          handleImageClick={handleImageClick}
          handleColorClick={handleColorClick}
          handleBgColorClick={handleBgColorClick}
          color={color}
          bgColor={bgColor}
          bgImage={image}
          handleFixedClick={handleFixedClick}
          fixedClasses={fixedClasses}
          sidebarMinimize={sidebarMinimize.bind(this)}
          miniActive={miniActive}
        /> */}
      </div>
    </div>
  );
}
