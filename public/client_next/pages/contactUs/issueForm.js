import React from "react";
import { Grid, Snackbar } from "@mui/material";
import { AddAlert } from "@mui/icons-material";
import { useForm } from "react-hook-form";
import { Row, Col, Form, Card } from "react-bootstrap";
import {
  getCountries,
  getIssueLocations,
  getIssueTypes,
  getIssuesSummary,
  getLookupValues,
  saveIssue,
} from "../api/contactUs";
import IssueFormAttachments from "../../Components/Issue/IssueFormAttachments";
import { useRouter } from "next/router";
import { getLocalByKey } from "../api/menu";
import IssuesSummary from "../../Components/Issue/IssuesSummary";

export default function IssueForm({ translations }) {
  const location = useRouter();
  const { locale } = location;

  const getLocal = (key) => {
    return getLocalByKey(key, translations, locale);
  };
  const getLocalByVal = (eng, local) => {
    return locale == "en" || !local ? eng : local;
  };

  const { register, handleSubmit, setValue, getValues, formState } = useForm();
  const [locations, setLocations] = React.useState([]);
  const [filterLocations, setFilterLocations] = React.useState([]);
  const [isLocationOther, setIsLocationOther] = React.useState(false);

  const onCountryChange = (value) => {
    getIssueLocations(value).then((data) => {
      setLocations(data.data.$values);
      if (getValues("typeOfLocationId")) {
        setFilterLocations(
          data.data.$values.filter(
            (l) => l.typeOfLocationId == getValues("typeOfLocationId")
          )
        );
      } else {
        setFilterLocations([]);
      }
    });
  };
  const [issuesList, setIssuesList] = React.useState([]);
  React.useEffect(() => {
    getIssuesSummary().then((data) => {
      setIssuesList(data.data.$values);
    });
  }, []);
  const [attachments, setAttachments] = React.useState([]);
  const [issueTypes, setIssueTypes] = React.useState([]);
  React.useEffect(() => {
    getIssueTypes().then((data) => {
      setIssueTypes(data.data.$values);
    });
  }, []);
  const [countries, setCountries] = React.useState([]);
  React.useEffect(() => {
    getCountries().then((data) => {
      setCountries(data.data.$values);
    });
  }, []);
  const [locationTypes, setLocationTypes] = React.useState([]);
  React.useEffect(() => {
    getLookupValues("00000000-0000-0000-0000-00000000002d").then((data) => {
      setLocationTypes(data.data.$values);
    });
  }, []);
  const [preferredHandlers, setPreferredHandlers] = React.useState([]);
  React.useEffect(() => {
    getLookupValues("00000000-0000-0000-0000-00000000002c").then((data) => {
      setPreferredHandlers(data.data.$values);
    });
  }, []);

  function onSubmit(inputData) {
    var formData = new FormData();
    for (const [key, value] of Object.entries(inputData)) {
      const dataValue = value && value[0] && value[0].name ? value[0] : value;
      formData.append(key, dataValue);
    }
    attachments.forEach((att) => {
      formData.append("AttachmentTitle", att.title);
      formData.append(att.title, att.file);
    });
    saveIssue(formData)
      .then((data) => {
        if (data.data.saved) {
          Reset();
          setErrorMsg(getLocal("Saved"));
          showNotification("tr");
        } else {
          if (data.data.erorMsg) {
            setErrorMsg(data.data.erorMsg);
          }
          showNotification("tr");
        }
      })
      .catch((error) => {
        // console.log(error);
        showNotification("tr");
      });
  }
  const Reset = () => {
    const values = getValues();
    for (const [key] of Object.entries(values)) {
      setValue(key, "");
    }
    setAttachments([]);
    setLocations([]);
    setFilterLocations([]);
  };
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
    <>
      <Grid container spacing={2} className="grid">
        <Grid item md={1}></Grid>
        <Grid item xs={12} md={10}>
          <Card>
            <Card.Body>
              <center><strong>{"Issues Summary By Type & Status"}</strong></center>
              <IssuesSummary
                issues={issuesList}
                issueTypes={issueTypes}
                getLocal={getLocal}
                getLocalByVal={getLocalByVal}
              />
            </Card.Body>
          </Card>
        </Grid>
        <Grid item md={1}></Grid>
      </Grid>
      <Grid container spacing={2} className="grid">
        <Grid item md={1}></Grid>
        <Grid item xs={12} md={10}>
          <div>
            <div className="bg">
              {getLocal("None Tariff Barrier Compliant")}
            </div>
          </div>
          <Form onSubmit={handleSubmit(onSubmit)} encType="multipart/form-data">
            <Row className="g-2">
              <Col md>
                <Form.Label>{getLocal("Company Name")}</Form.Label>
                <Form.Control
                  type="text"
                  {...register("companyName", {
                    required: true,
                    maxLength: 255,
                  })}
                  style={{
                    borderColor: formState.errors["companyName"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label>{getLocal("Type")}</Form.Label>
                <Form.Select
                  {...register("typeId", { required: true })}
                  style={{
                    borderColor: formState.errors["contactType"] ? "red" : "",
                  }}
                >
                  <option></option>
                  {issueTypes.map((value, index) => (
                    <option value={value.id} key={index}>
                      {getLocalByVal(value.name, value.nameLocal)}
                    </option>
                  ))}
                </Form.Select>
              </Col>
              <Col md>
                <Form.Label>{getLocal("Date Of Occurrence")}</Form.Label>
                <Form.Control
                  type="date"
                  {...register("dateOfOccurrence", { required: true })}
                  style={{
                    borderColor: formState.errors["dateOfOccurrence"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
            </Row>
            <Row>
              <Col md>
                <Form.Label> {getLocal("Details Of Occurrence")}</Form.Label>
                <Form.Control
                  as="textarea"
                  rows="2"
                  {...register("detailsOfOccurrence", {
                    required: true,
                    maxLength: 1000,
                  })}
                  style={{
                    borderColor: formState.errors["detailsOfOccurrence"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
            </Row>
            <Row className="g-2">
              <Col md>
                <Form.Label>{getLocal("Measure Code")}</Form.Label>
                <Form.Control
                  type="text"
                  {...register("measureCode", {
                    required: true,
                    maxLength: 255,
                  })}
                  style={{
                    borderColor: formState.errors["measureCode"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label> {getLocal("Place Of Occurrence")}</Form.Label>
                <Form.Control
                  type="text"
                  {...register("placeOfOccurrence", {
                    required: true,
                    maxLength: 255,
                  })}
                  style={{
                    borderColor: formState.errors["placeOfOccurrence"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label>
                  {getLocal("Imposing Country or Region")}
                </Form.Label>
                <Form.Select
                  {...register("countryRegion", { required: true })}
                  onChange={(evt) => onCountryChange(evt.target.value)}
                  style={{
                    borderColor: formState.errors["countryRegion"] ? "red" : "",
                  }}
                >
                  <option></option>
                  {countries.map((value, index) => (
                    <option value={value.name} key={index}>
                      {value.name}
                    </option>
                  ))}
                </Form.Select>
              </Col>
            </Row>
            <Row>
              <Col md={4}>
                <Form.Label> {getLocal("Type of Location")}</Form.Label>
                <Form.Select
                  {...register("typeOfLocationId", { required: true })}
                  onChange={(evt) => {
                    setFilterLocations(
                      locations.filter(
                        (l) => l.typeOfLocationId == evt.target.value
                      )
                    );
                  }}
                  style={{
                    borderColor: formState.errors["typeOfLocationId"]
                      ? "red"
                      : "",
                  }}
                >
                  <option></option>
                  {locationTypes.map((value, index) => (
                    <option value={value.id} key={index}>
                      {value.name}
                    </option>
                  ))}
                </Form.Select>
              </Col>
              <Col md={4}>
                <Form.Label> {getLocal("Location")}</Form.Label>
                <Form.Select
                  {...register("locationId", { required: true })}
                  onChange={(evt) =>
                    setIsLocationOther(evt.target.value == "Other")
                  }
                  style={{
                    borderColor: formState.errors["locationId"] ? "red" : "",
                  }}
                >
                  <option></option>
                  {filterLocations.map((value, index) => (
                    <option value={value.id} key={index}>
                      {value.name}
                    </option>
                  ))}
                  <option value={null}> {getLocal("Other")}</option>
                </Form.Select>
              </Col>
              {isLocationOther ? (
                <Col md={4}>
                  <Form.Label> {getLocal("Other Location")}</Form.Label>
                  <Form.Control
                    type="text"
                    {...register("otherLocation", {
                      required: true,
                      maxLength: 255,
                    })}
                    style={{
                      borderColor: formState.errors["otherLocation"]
                        ? "red"
                        : "",
                    }}
                  />
                </Col>
              ) : null}
            </Row>
            <Row>
              <Col md>
                <Form.Label>
                  {getLocal("Description of the Incident")}
                </Form.Label>
                <Form.Control
                  as="textarea"
                  rows="2"
                  {...register("description", {
                    required: true,
                    maxLength: 600,
                  })}
                  style={{
                    borderColor: formState.errors["description"] ? "red" : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label>
                  {getLocal("Details of affected Product/s")}
                </Form.Label>
                <Form.Control
                  as="textarea"
                  rows="2"
                  {...register("affectedProducts", {
                    required: true,
                    maxLength: 600,
                  })}
                  style={{
                    borderColor: formState.errors["affectedProducts"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
            </Row>
            <Row>
              <Col md>
                <Form.Label>{getLocal("Product/s tariff code")}</Form.Label>
                <Form.Control
                  type="text"
                  {...register("productTariffCode", {
                    required: true,
                    maxLength: 255,
                  })}
                  style={{
                    borderColor: formState.errors["productTariffCode"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label>{getLocal("Product/s Value Currency")}</Form.Label>
                <Form.Control
                  type="text"
                  {...register("productValueCurrency", {
                    required: true,
                    maxLength: 255,
                  })}
                  style={{
                    borderColor: formState.errors["productValueCurrency"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md>
                <Form.Label>{getLocal("Product/s Value Amount")}</Form.Label>
                <Form.Control
                  type="number"
                  {...register("productValueAmount", {
                    required: true,
                    maxLength: 255,
                  })}
                  style={{
                    borderColor: formState.errors["productValueAmount"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
            </Row>
            <Row>
              <Col md={8}>
                <Form.Label>{getLocal("Description of product/s")}</Form.Label>
                <Form.Control
                  as="textarea"
                  rows="3"
                  {...register("descriptionOfProducts", {
                    required: true,
                    maxLength: 600,
                  })}
                  style={{
                    borderColor: formState.errors["descriptionOfProducts"]
                      ? "red"
                      : "",
                  }}
                />
              </Col>
              <Col md={4}>
                <Form.Label>
                  {getLocal("Government/agency Preferred to handle Complaint")}
                </Form.Label>
                <Form.Select
                  {...register("preferredHandlerId", { required: true })}
                  style={{
                    borderColor: formState.errors["typeOfLocationId"]
                      ? "red"
                      : "",
                  }}
                >
                  <option></option>
                  {preferredHandlers.map((value, index) => (
                    <option value={value.id} key={index}>
                      {value.name}
                    </option>
                  ))}
                </Form.Select>
              </Col>
            </Row>
            <Row>
              <Col>
                <IssueFormAttachments
                  attachments={attachments}
                  setAttachments={setAttachments}
                  showNotification={showNotification}
                  setErrorMsg={setErrorMsg}
                  getLocal={getLocal}
                />
              </Col>
            </Row>
            <button type="submit" className="btn btn-success">
              {getLocal("Save")}
            </button>
            <br />
          </Form>
        </Grid>
        <Grid item md={1}></Grid>
      </Grid>
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
    </>
  );
}
