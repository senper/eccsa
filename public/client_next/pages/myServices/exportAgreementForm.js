import React from "react";
import { useForm } from "react-hook-form";
import {
  getExportAgreement,
  saveExportAgreement,
} from "../../api/exportAgreement";
import { Row, Col, Form } from "react-bootstrap";
import { useRouter } from "next/router";
import { getLocalByKey } from "../api/menu";
import { Grid, Snackbar } from "@mui/material";
import { useSelector } from "react-redux";
import { AddAlert } from "@mui/icons-material";
import Custom404 from "../404";
import Login from "../login";

const emptyGuid = "00000000-0000-0000-0000-000000000000";
export default function ExportAgreementForm({ translations }) {
  const [user, setUser] = React.useState(
    useSelector((state) => state.auth.user)
  );

  const location = useRouter();
  const { locale } = location;
  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };
  const {
    register,
    handleSubmit,
    setValue,
    getValues,
    formState, //: { errors },
  } = useForm();
  let idValue = null;
  if (location.query) {
    const { id } = location.query;
    idValue = id;
  }
  const [readOnly, setReadOnly] = React.useState("");
  React.useEffect(() => {
    if (user && idValue) {
      getExportAgreement(idValue).then((data) => {
        const values = getValues();
        for (const [key] of Object.entries(values)) {
          setValue(key, data.data[key]);
        }
        const canEdit = !idValue || !data.data || !data.data.isLocked;
        setReadOnly(canEdit ? "" : "readOnly");
      });
    }
  }, [user, idValue]);
  function onSubmit(inputData) {
    inputData.id = idValue ? idValue : emptyGuid
    saveExportAgreement(inputData)
      .then((data) => {
        if (data.data.saved) {
          Redirect();
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        // console.log(error.response);
        showNotification("tr");
      });
  }
  function Redirect() {
    location.push("/myServices?navTo=exportAgreement");
  }
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
  
  if (!user) {
    //return <Custom404 statusCode={404} />;
    return <Login translations={translations} />;
  }
  return (
    <div>
      <Grid container spacing={2}>
        <Grid item xs={1} md={3}></Grid>
        <Grid item xs={11} md={6}>
          <div className="row">
            <div className="bg">
              {idValue ? getLocal("Update") : getLocal("Add")}{" "}
              {getLocal("Export Agreement")}
            </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
            <input type="hidden" {...register("id")} />
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Agreement No")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("agreementNo", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["agreementNo"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("License No")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="text"
                  {...register("licenseNo", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["licenseNo"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Consignee")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("consignee", { required: true, maxLength: 255 })}
                  style={{
                    borderColor: formState.errors["consignee"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label>{getLocal("Type Of Carriage")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="text"
                  {...register("typeOfCarriage", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["typeOfCarriage"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Country")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="text"
                  {...register("country", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["country"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("City")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("city", { required: true, maxLength: 50 })}
                  style={{
                    borderColor: formState.errors["city"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Address")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("address", { required: true, maxLength: 255 })}
                  style={{
                    borderColor: formState.errors["address"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("County Of Destination")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("countyOfDestination", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["countyOfDestination"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("Port Of Destination")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="text"
                  {...register("portOfDestination", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["portOfDestination"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Port Of Departure")}</Form.Label>
                <Form.Control
                  type="text"
                  {...{ readOnly }}
                  {...register("portOfDeparture", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["portOfDeparture"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("Name Of Carrier")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="text"
                  {...register("nameOfCarrier", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["nameOfCarrier"] ? "red" : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label> {getLocal("Term Of Delivery")} </Form.Label>
                <Form.Control
                  {...{ readOnly }}
                  type="text"
                  {...register("termOfDelivery", {
                    required: true,
                    maxLength: 50,
                  })}
                  style={{
                    borderColor: formState.errors["termOfDelivery"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("Status")}</Form.Label>
                <Form.Select
                  {...register("status", { required: true })}
                  style={{
                    borderColor: formState.errors["status"] ? "red" : "",
                  }}
                >
                  <option value="Active">{getLocal("Active")}</option>
                  <option value="Inactive">{getLocal("Inactive")}</option>
                </Form.Select>
              </Col>
            </Row>
            <button type="submit" className="btn btn-success">
            {getLocal("Save")}
            </button>
            <button type="button" className="btn btn-warning" onClick={Redirect}>
            {getLocal("Cancel")}
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item xs={1} md={3} ></Grid>
        <Grid container spacing={2}>
          <br />
          <br />
        </Grid>
        <Snackbar
          place="tr"
          color="danger"
          icon={AddAlert}
          message={errorMsg ? errorMsg : getLocal("Failed to Save")}
          open={error}
          onClose={() => setError(false)}
        />
      </Grid>
    </div>
  );
}

// export default App;
