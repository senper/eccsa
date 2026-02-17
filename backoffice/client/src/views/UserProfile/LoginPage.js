/* eslint-disable react/prop-types */
import React from "react";
import { useForm } from "react-hook-form";
import { login } from "services/oauth";
import { useHistory, useLocation } from "react-router-dom";
import { useSelector } from "react-redux";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
import InputAdornment from "@material-ui/core/InputAdornment";
import Icon from "@material-ui/core/Icon";

// @material-ui/icons
import Email from "@material-ui/icons/Email";

// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import CustomInput from "components/vender/CustomInput/CustomInput.js";
import Button from "components/vender/CustomButtons/Button.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import CardFooter from "components/vender/Card/CardFooter.js";
import Snackbar from "components/vender/Snackbar/Snackbar.js";
import AddAlert from "@material-ui/icons/AddAlert";

import styles from "assets/jss/material-dashboard-pro-react/views/loginPageStyle.js";
import { logoff } from "services/oauth";
import { loadUserFromStorage } from "services/oauth";
import store from "store";
import { useMutation, useQuery } from "urql";
import { setAuthHeader } from "utils/axiosHeaders";
import { SIGNUP_MUTATION, USER_PROFILE_QUERY } from "variables/user_gql";
import { Modules } from "variables/lookups.js";

const useStyles = makeStyles(styles);

export default function LoginPage() {
  // var user = useSelector((state) => state.auth.user);
  const history = useHistory();
  const location = useLocation();
  const [cardAnimaton, setCardAnimation] = React.useState("cardHidden");
  const { register: loginFrom, handleSubmit } = useForm();
  const [error, setError] = React.useState(false);
  const [errorMgs, setErrorMgs] = React.useState("");
  React.useEffect(() => {
    if (location.state && location.state.error) {
      setErrorMgs(location.state.error);
      showNotification("tr");
    }
  }, [location.state]);
  var token = localStorage.getItem("access_token");
  React.useEffect(() => {
    let id = setTimeout(function () {
      setCardAnimation("");
    }, 700);
    // Specify how to clean up after this effect:
    return function cleanup() {
      window.clearTimeout(id);
    };
  });
  const classes = useStyles();

  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMgs(undefined);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };

  const [{ fetching }, executeMutation] = useMutation(SIGNUP_MUTATION);
  const [result, executeQuery] = useQuery({
    query: USER_PROFILE_QUERY,
    pause: true,
  });
  React.useEffect(() => {
    if (result.data) {
      const user = result.data.userProfile;
      //console.log(user);
      if (user && result.error) {
        setErrorMgs(result.error.message);
        showNotification("tr");
      } else if (!user) {
        //logoff();
        localStorage.removeItem("access_token");
        token = undefined;
      } else if (user && token) {
        redirect(user.defaultModule);
      }
    } else if (result.error) {
      setErrorMgs("Unable to get user profile");
      showNotification("tr");
    }
  }, [result.data, result.error]);

  const onSubmitLogin = (data) => {
    executeMutation({ email: data.email, password: data.password })
      .then(({ data }) => {
        if (data.login) {
          token = data.login.token;
          // console.log(token);
          if (token) {
            localStorage.setItem("access_token", token);
            executeQuery();
          }
        } else {
          showNotification("tr");
        }
      })
      .catch((error) => {
        console.log(error);
        showNotification("tr");
      });

    /* login({ email: data.email, password: data.password })
      .then((token) => {
        if (token) {
          if (!user) {
            loadUserFromStorage(store).then((result) => {
              console.log(result);
              if (result == "Error") {
                logoff();
                token = undefined;
              } else {
                redirect(result.roles);
              }
            });
          } else if (user) {
            redirect(user.roles);
          }
        } else {
          showNotification("tr");
        }
      })
      .catch((error) => {
        console.log(error);
        showNotification("tr");
      }); */
  };

  const redirect = (defaultModule) => {
    var module = Modules.find((item) => {
      return item.id == defaultModule;
    });
    if (module) {
      history.push(module.layout);
    }
    // if (roles.includes("Membership")) {
    //   history.push("/membership");
    // } else if (roles.includes("Trade Promotion")) {
    //   history.push("/tradePromotion");
    // } else if (roles.includes("Academy")) {
    //   history.push("/academy");
    // } else if (roles.includes("Admin")) {
    //   history.push("/configuration");
    // } else {
    //   history.push("/membership");
    // }
  };

  return (
    <div className={classes.container}>
      <GridContainer justifyContent="center">
        <GridItem xs={12} sm={6} md={4}>
          <form onSubmit={handleSubmit(onSubmitLogin)} key={1}>
            <Card login className={classes[cardAnimaton]}>
              <CardHeader
                className={`${classes.cardHeader} ${classes.textCenter}`}
                color="rose"
              >
                <h4 className={classes.cardTitle}>Log in</h4>
                <div className={classes.socialLine}>
                  {/* {[
                    "fab fa-facebook-square",
                    "fab fa-twitter",
                    "fab fa-google-plus",
                  ].map((prop, key) => {
                    return (
                      <Button
                        color="transparent"
                        justIcon
                        key={key}
                        className={classes.customButtonClass}
                      >
                        <i className={prop} />
                      </Button>
                    );
                  })} */}
                </div>
              </CardHeader>
              <CardBody>
                <CustomInput
                  inputRef={loginFrom({ required: true })}
                  labelText="Email..."
                  id="email"
                  name="email"
                  formControlProps={{
                    fullWidth: true,
                  }}
                  inputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <Email className={classes.inputAdornmentIcon} />
                      </InputAdornment>
                    ),
                  }}
                />
                <CustomInput
                  inputRef={loginFrom({ required: true })}
                  labelText="Password"
                  id="password"
                  name="password"
                  formControlProps={{
                    fullWidth: true,
                  }}
                  inputProps={{
                    endAdornment: (
                      <InputAdornment position="end">
                        <Icon className={classes.inputAdornmentIcon}>
                          lock_outline
                        </Icon>
                      </InputAdornment>
                    ),
                    type: "password",
                    autoComplete: "off",
                  }}
                />
              </CardBody>
              {/* <CardFooter className={classes.justifyContentCenter}>
                <p>
                  The default users are <strong>alice/bob</strong>, password:{" "}
                  <strong>Pass123$</strong>
                </p>
              </CardFooter> */}
              <CardFooter className={classes.justifyContentCenter}>
                <Button type="submit" color="rose" simple size="lg" block>
                  Let{"'"}s Go
                </Button>
                <Snackbar
                  place="tr"
                  color="danger"
                  icon={AddAlert}
                  message={errorMgs ? errorMgs : "Failed to authenticate"}
                  open={error}
                  closeNotification={() => setError(false)}
                  close
                />
              </CardFooter>
            </Card>
          </form>
        </GridItem>
      </GridContainer>
    </div>
  );
}
