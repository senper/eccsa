import React from "react";

import { makeStyles } from "@material-ui/core/styles";
import { Grid, Paper, Snackbar } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.min.css";
import "../../App.css";
import "../../pages/Pages.css";
import "../slider/HomeSlider";
import { Link } from "react-router-dom";
import { Breadcrumbs } from "@material-ui/core";
import "bootstrap/dist/css/bootstrap.css";
import Footer from "../Footer/Footer";
import Maps from "../Map/Map";
import { saveContactUs } from "../../services/contactUs";
import { useForm } from "react-hook-form";
import { AddAlert } from "@material-ui/icons";

const useStyle = makeStyles((theme) => ({
  grid: {
    width: "100%",
    margin: "0px",
    padding: "3px",
  },
  paper: {
    padding: theme.spacing(2),
    textAlign: "center",
    color: theme.palette.text.secondary,
    background: theme.palette.success.light,
  },
  breadcrum: {
    backgroundColor: "#fff",
    padding: "10px",
    marginLeft: "100px",
  },
}));

export default function ContactPage() {
  const classes = useStyle();
  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState, //: { errors },
  } = useForm();
  ///
  const onSubmit = (inputData) => {
    saveContactUs(inputData)
      .then((data) => {
        if (data.data.saved) {
          Reset();
          setErrorMsg("Submitted!!");
          showNotification("tr");
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        console.log(error.response);
        showNotification("tr");
      });
  };
  const Reset = () => {
    const values = getValues();
    for (const [key, ] of Object.entries(values)) {
      setValue(key, "");
    }
  };
  ///
  const [error, setError] = React.useState(false);
  const [errorMsg, setErrorMsg] = React.useState(null);
  const showNotification = (place) => {
    switch (place) {
      case "tr":
        if (!error) {
          setError(true);
          setTimeout(function () {
            setError(false);
            setErrorMsg(null);
          }, 6000);
        }
        break;
      default:
        break;
    }
  };
  return (
    <div class="mmenu">
      <Grid container spacing={2} className="grid">
        <Grid item xs={12} md={12}>
          <Breadcrumbs aria-label="breadcrumb" className={classes.breadcrum}>
            <Link
              color="inherit"
              to={{
                pathname: "/",
                state: {
                  eventKey: "default",
                },
              }}
            >
              Home
            </Link>
            <Link color="textPrimary" to="#" aria-current="page">
              Contact Us
            </Link>
          </Breadcrumbs>
        </Grid>
        <Grid item xs={1} md={1}></Grid>
        <Grid item xs={12} md={12}>
          <Paper>
            <div class="container">
              <div class="row">
                <div class="col-md-6">
                  {
                    <form onSubmit={handleSubmit(onSubmit)}>
                      <div class="form-group">
                        <label for="name">Name</label>
                        <input
                          type="text"
                          class="form-control"
                          id="name"
                          placeholder="Enter name"
                          {...register("name", {
                            required: true,
                            maxLength: 50,
                          })}
                          style={{
                            borderColor: formState.errors["name"] ? "red" : "",
                          }}
                        />
                      </div>
                      <div class="form-group">
                        <label for="email">Email Address</label>
                        <div class="input-group">
                          <span class="input-group-addon">
                            <span class="glyphicon glyphicon-envelope"></span>
                          </span>
                          <input
                            type="email"
                            class="form-control"
                            id="email"
                            placeholder="Enter email"
                            {...register("email", {
                              required: true,
                              maxLength: 50,
                            })}
                            style={{
                              borderColor: formState.errors["email"]
                                ? "red"
                                : "",
                            }}
                          />
                        </div>
                      </div>
                      <div class="form-group">
                        <label for="name">Message</label>
                        <textarea
                          name="message"
                          id="message"
                          class="form-control"
                          rows="9"
                          required="required"
                          placeholder="Message"
                          {...register("message", {
                            required: true,
                          })}
                          style={{
                            borderColor: formState.errors["message"]
                              ? "red"
                              : "",
                          }}
                        ></textarea>
                      </div>
                      <div class="form-group">
                        <button
                          type="submit"
                          class="btn btn-primary pull-right"
                          id="btnContactUs"
                        >
                          Send Message
                        </button>
                      </div>
                    </form>
                  }
                </div>
                <div class="col-md-6">
                  <form>
                    <legend>
                      <span class="glyphicon glyphicon-globe"></span> Our office
                    </legend>
                  </form>
                  <Maps />
                </div>
              </div>
            </div>
          </Paper>
        </Grid>

        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : "Failed to Save"}
          open={error}
          closeNotification={() => setError(false)}
          close
        />
      </Grid>
    </div>
  );
}
