/* eslint-disable react/prop-types */
import React from "react";
import { useHistory } from "react-router-dom";
import { useLocation } from "react-router-dom";
import classNames from "classnames";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import MenuItem from "@material-ui/core/MenuItem";
import MenuList from "@material-ui/core/MenuList";
import ClickAwayListener from "@material-ui/core/ClickAwayListener";
import Paper from "@material-ui/core/Paper";
import Grow from "@material-ui/core/Grow";
import Popper from "@material-ui/core/Popper";
import Button from "components/vender/CustomButtons/Button.js";

import styles from "assets/jss/material-dashboard-pro-react/components/adminNavbarLinksStyle.js";
import { ViewModule } from "@material-ui/icons";
import { Modules } from "variables/lookups.js";

const useStyles = makeStyles(styles);

export default function CustomAdminNavbarLinks(props) {
  const location = useLocation();
  const history = useHistory();

  const [userModules, setUserModules] = React.useState([]);
  React.useEffect(() => {
    if (props.loginUser && props.loginUser.assignedModules) {
      setUserModules(
        Modules.filter(
          (module) => (module.id & props.loginUser.assignedModules) == module.id
        )
      );
    }
  }, [props.loginUser]);

  const [openModuleMenu, setOpenModuleMenu] = React.useState(null);
  const handleClickModuleMenu = (event) => {
    if (openModuleMenu && openModuleMenu.contains(event.target)) {
      setOpenModuleMenu(null);
    } else {
      setOpenModuleMenu(event.currentTarget);
    }
  };
  const handleCloseModuleMenu = () => {
    setOpenModuleMenu(null);
  };
  const handleModuleMenu = (module) => {
    history.push(module.layout);
  };
  ///
  const classes = useStyles();
  const dropdownItem = classNames(classes.dropdownItem, classes.primaryHover, {
    [classes.dropdownItemRTL]: false,
  });
  const wrapper = classNames({
    [classes.wrapperRTL]: false,
  });
  const managerClasses = classNames({
    [classes.managerClasses]: true,
  });
  return (
    <>
      {userModules && userModules.length > 0 ? (
        <div className={wrapper}>
          <div className={managerClasses}>
            <Button
              color="transparent"
              aria-label="Module"
              justIcon
              aria-owns={openModuleMenu ? "module-menu-list" : null}
              aria-haspopup="true"
              onClick={handleClickModuleMenu}
              className={classes.buttonLink}
              muiClasses={{
                label: "",
              }}
            >
              <ViewModule
                className={classes.headerLinksSvg + " " + classes.links}
              />
            </Button>
            <Popper
              open={Boolean(openModuleMenu)}
              anchorEl={openModuleMenu}
              transition
              disablePortal
              placement="bottom"
              className={classNames({
                [classes.popperClose]: !openModuleMenu,
                [classes.popperResponsive]: true,
                [classes.popperNav]: true,
              })}
            >
              {({ TransitionProps }) => (
                <Grow
                  {...TransitionProps}
                  id="module-menu-list"
                  style={{ transformOrigin: "0 0 0" }}
                >
                  <Paper className={classes.dropdown}>
                    <ClickAwayListener onClickAway={handleCloseModuleMenu}>
                      <MenuList role="menu">
                        {userModules.map((module) => {
                          return (
                            <MenuItem
                              key={module.id}
                              onClick={() => handleModuleMenu(module)}
                              className={dropdownItem}
                            >
                              {module.name}
                            </MenuItem>
                          );
                        })}
                      </MenuList>
                    </ClickAwayListener>
                  </Paper>
                </Grow>
              )}
            </Popper>
          </div>
        </div>
      ) : null}
    </>
  );
}
