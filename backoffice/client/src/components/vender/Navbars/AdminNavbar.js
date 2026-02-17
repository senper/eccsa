/* eslint-disable react/prop-types */
import React from "react";
// nodejs library to set properties for components
import PropTypes from "prop-types";
import cx from "classnames";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import AppBar from "@material-ui/core/AppBar";
import Toolbar from "@material-ui/core/Toolbar";
import Hidden from "@material-ui/core/Hidden";

// material-ui icons
import Menu from "@material-ui/icons/Menu";
import MoreVert from "@material-ui/icons/MoreVert";
import ViewList from "@material-ui/icons/ViewList";

// core components
import AdminNavbarLinks from "./AdminNavbarLinks";
import Button from "components/vender/CustomButtons/Button.js";

import styles from "assets/jss/material-dashboard-pro-react/components/adminNavbarStyle.js";
import CustomAdminNavbarLinks from "components/common/AdminNavbar/CustomAdminNavbarLinks";

const useStyles = makeStyles(styles);

export default function AdminNavbar(props) {
  const classes = useStyles();
  const { navbarColor, rtlActive, brandText } = props;
  const appBarClasses = cx({
    [" " + classes[navbarColor]]: navbarColor,
  });
  const sidebarMinimize =
    classes.sidebarMinimize +
    " " +
    cx({
      [classes.sidebarMinimizeRTL]: rtlActive,
    });
  return (
    <AppBar className={classes.appBar + appBarClasses}>
      <Toolbar className={classes.container}>
        <Hidden smDown implementation="css">
          <div className={sidebarMinimize}>
            {props.miniActive ? (
              <Button
                justIcon
                round
                color="white"
                onClick={props.sidebarMinimize}
              >
                <ViewList className={classes.sidebarMiniIcon} />
              </Button>
            ) : (
              <Button
                justIcon
                round
                color="white"
                onClick={props.sidebarMinimize}
              >
                <MoreVert className={classes.sidebarMiniIcon} />
              </Button>
            )}
          </div>
        </Hidden>
        <div className={classes.flex}>
          <p
            style={{
              paddingTop: "15px",
            }}
          >
            <span
              style={{
                paddingLeft: "15px",
                paddingTop: "100px",
                fontSize: "30px",
                fontWeight: "bolder",
              }}
            >
              {props.moduleName2}
            </span>{" "}
            /{" "}
            <span
              style={{
                fontSize: "15px",
                fontWeight: "normal",
              }}
            >
              {brandText}
            </span>
          </p>
          {/* <Button
            href="javascript:void(0)"
            className={classes.title}
            color="transparent"
          >
            {props.moduleName2}
          </Button> */}
          {/* Here we create navbar brand, based on route name */}
          {/* <Button
            href="javascript:void(0)"
            className={classes.title}
            color="transparent"
          >
            {brandText}
          </Button> */}
        </div>
        <Hidden smDown implementation="css">
          {/* <AdminNavbarLinks rtlActive={rtlActive} loginUser={props.loginUser} /> */}
          <CustomAdminNavbarLinks loginUser={props.loginUser} />
        </Hidden>
        <Hidden mdUp implementation="css">
          <Button
            className={classes.appResponsive}
            color="transparent"
            justIcon
            aria-label="open drawer"
            onClick={props.handleDrawerToggle}
          >
            <Menu />
          </Button>
        </Hidden>
      </Toolbar>
    </AppBar>
  );
}

AdminNavbar.propTypes = {
  // color: PropTypes.oneOf(["primary", "info", "success", "warning", "danger"]),
  rtlActive: PropTypes.bool,
  brandText: PropTypes.string,
  miniActive: PropTypes.bool,
  handleDrawerToggle: PropTypes.func,
  sidebarMinimize: PropTypes.func,
};
