/* eslint-disable react/prop-types */
import React from "react";

// @material-ui/core components
import { makeStyles } from "@material-ui/core/styles";
// core components
import GridContainer from "components/vender/Grid/GridContainer.js";
import GridItem from "components/vender/Grid/GridItem.js";
import Card from "components/vender/Card/Card.js";
import CardBody from "components/vender/Card/CardBody.js";
import CardIcon from "components/vender/Card/CardIcon.js";
import CardHeader from "components/vender/Card/CardHeader.js";
import Button from "components/vender/CustomButtons/Button.js";
import btnstyles from "assets/jss/material-dashboard-pro-react/views/buttonsStyle.js";
import {
  Dialog,
  DialogActions,
  DialogContent,
  DialogTitle,
  FormControl,
  InputLabel,
  Link,
  MenuItem,
  Select,
  Slide,
} from "@material-ui/core";
import styles from "assets/jss/material-dashboard-pro-react/views/notificationsStyle.js";

import { Close } from "@material-ui/icons";
import { Controller, useForm } from "react-hook-form";
import { TOPIC_CATEGORY_QUERY } from "variables/academy/topic_category_gql";
import { useMutation, useQuery } from "urql";
import CustomInput from "components/vender/CustomInput/CustomInput";
import { TOPIC_UPDATE_MUTATION } from "variables/academy/topics_gql";
import { statuses } from "variables/lookups";
import { TOPIC_CREATE_MUTATION } from "variables/academy/topics_gql";

const useBtnStyles = makeStyles(btnstyles);
const useStyles = makeStyles(styles);

const Transition = React.forwardRef(function Transition(props, ref) {
  return <Slide direction="down" ref={ref} {...props} />;
});

export default function EditTopicModal({
  openModal,
  setOpenModal,
  topic,
  setTopic,
  reloadParentData,
  setMessageProps,
}) {
  const classes = useStyles();
  //
  const [categories, setCategories] = React.useState([]);
  const [result, executeQuery] = useQuery({
    query: TOPIC_CATEGORY_QUERY,
  });
  const {
    register: editTopicForm,
    handleSubmit,
    control,
    setValue,
    getValues,
  } = useForm({
    mode: "onChange",
    defaultValues: topic,
  });
  React.useEffect(() => {
    if (result.data && result.data.categories) {
      const categoriesData = result.data.categories;
      setCategories(categoriesData);
      //console.log(categoriesData);
    }
  }, [result.data, openModal]);
  //
  React.useEffect(() => {
    const values = getValues();
    //console.log(values);
    //console.log(getValues("code"));
    for (const property in values) {
      setValue(property, values[property]);
    }
  }, [openModal]);
  //
  const [{ fetching }, executeMutation] = useMutation(
    topic.id ? TOPIC_UPDATE_MUTATION : TOPIC_CREATE_MUTATION
  );
  const onSubmitLogin = (updatedData) => {
    //console.log(updatedData);
    executeMutation({ topic: updatedData }).then(({ data, error }) => {
      //console.log(data);
      if (error) {
        setMessageProps({
          color: "danger",
          open: true,
          message: error?.message,
        });
      } else {
        if (setTopic) {
          const values = getValues();
          topic = {
            ...topic,
            ...values,
            category: categories.find(
              (item) => item.id == values["categoryId"]
            ),
          };
          setTopic(topic);
        }
        setOpenModal(false);
        reloadParentData();
        var addOrEdit = topic.id ? "Updated" : "Created";
        setMessageProps({
          color: "success",
          open: true,
          message: `Successfully ${addOrEdit}`,
        });
      }
    });
  };
  //
  return (
    <>
      <Dialog
        classes={{
          root: classes.center + " " + classes.modalRoot,
          paper: classes.modal,
        }}
        open={openModal}
        //TransitionComponent={Transition}
        keepMounted
        onClose={(event, reason) => {
          if (reason !== "backdropClick") {
            setOpenModal(false);
          }
        }}
        aria-labelledby="classic-modal-slide-title"
        aria-describedby="classic-modal-slide-description"
      >
        <DialogTitle
          id="classic-modal-slide-title"
          disableTypography
          className={classes.modalHeader}
          style={{ backgroundColor: "lightblue" }}
        >
          <Button
            justIcon
            className={classes.modalCloseButton}
            key="close"
            aria-label="Close"
            color="transparent"
            onClick={() => setOpenModal(false)}
          >
            <Close className={classes.modalClose} />
          </Button>
          <h4 className={classes.modalTitle}>
            {topic.id ? "Edit Topic" : "Add Topic"}
          </h4>
        </DialogTitle>
        <form onSubmit={handleSubmit(onSubmitLogin)} key={1}>
          <DialogContent
            dividers
            id="classic-modal-slide-description"
            className={classes.modalBody}
          >
            <GridContainer>
              <GridItem xs={12} sm={12} md={12}>
                {topic.id ? (
                  <input type="hidden" ref={editTopicForm()} name="id" />
                ) : null}
                <CustomInput
                  inputRef={editTopicForm({ required: true })}
                  labelText="Code"
                  id="code"
                  name="code"
                  formControlProps={{
                    fullWidth: true,
                  }}
                />
                <CustomInput
                  inputRef={editTopicForm({ required: true })}
                  labelText="Name"
                  id="nameEng"
                  name="nameEng"
                  formControlProps={{
                    fullWidth: true,
                  }}
                />
                <CustomInput
                  inputRef={editTopicForm({ required: true })}
                  labelText="Local"
                  id="nameLocal"
                  name="nameLocal"
                  formControlProps={{
                    fullWidth: true,
                  }}
                />
                <FormControl fullWidth className={classes.labelHorizontal}>
                  <InputLabel
                    htmlFor={`categoryId`}
                    className={classes.labelHorizontal}
                  >
                    Status
                  </InputLabel>
                  <Controller
                    control={control}
                    name={`status`}
                    render={({ onChange, value, name }) => (
                      <Select
                        style={{ textAlign: "left" }}
                        value={value ? value : ""}
                        name={name}
                        id={`status`}
                        MenuProps={{
                          className: classes.selectMenu,
                        }}
                        classes={{
                          select: classes.select,
                        }}
                        onChange={(event) => {
                          onChange(event);
                        }}
                      >
                        <MenuItem
                          disabled
                          classes={{
                            root: classes.selectMenuItem,
                          }}
                        >
                          Choose Status
                        </MenuItem>
                        {statuses.map((_item, index) => {
                          return (
                            <MenuItem
                              key={_item.id}
                              classes={{
                                root: classes.selectMenuItem,
                                selected: classes.selectMenuItemSelected,
                              }}
                              value={_item.id}
                            >
                              {_item.name}
                            </MenuItem>
                          );
                        })}
                      </Select>
                    )}
                  />
                </FormControl>
                <br />
                <br />
                <FormControl fullWidth className={classes.labelHorizontal}>
                  <InputLabel
                    htmlFor={`categoryId`}
                    className={classes.labelHorizontal}
                  >
                    Category
                  </InputLabel>
                  <Controller
                    control={control}
                    name={`categoryId`}
                    render={({ onChange, value, name }) => (
                      <Select
                        style={{ textAlign: "left" }}
                        value={value ? value : ""}
                        name={name}
                        id={`categoryId`}
                        MenuProps={{
                          className: classes.selectMenu,
                        }}
                        classes={{
                          select: classes.select,
                        }}
                        onChange={(event) => {
                          onChange(event);
                        }}
                      >
                        <MenuItem
                          disabled
                          classes={{
                            root: classes.selectMenuItem,
                          }}
                        >
                          Choose Category
                        </MenuItem>
                        {categories.map((_item, index) => {
                          return (
                            <MenuItem
                              key={_item.id}
                              classes={{
                                root: classes.selectMenuItem,
                                selected: classes.selectMenuItemSelected,
                              }}
                              value={_item.id}
                            >
                              {_item.code} {_item.nameEng}({_item.nameLocal})
                            </MenuItem>
                          );
                        })}
                      </Select>
                    )}
                  />
                </FormControl>
                <br />
                <br />
                <FormControl
                  style={{ textAlign: "left" }}
                  fullWidth
                  className={classes.labelHorizontal}
                >
                  <label>Description</label>
                  <textarea
                    ref={editTopicForm({ required: true })}
                    rows={3}
                    placeholder="Description"
                    name="decription"
                  ></textarea>
                </FormControl>
                <br />
                <br />
                <FormControl
                  style={{ textAlign: "left" }}
                  fullWidth
                  className={classes.labelHorizontal}
                >
                  <label>Local</label>
                  <textarea
                    ref={editTopicForm({ required: true })}
                    rows={3}
                    placeholder=""
                    name="decriptionLocal"
                  ></textarea>
                </FormControl>
              </GridItem>
            </GridContainer>
          </DialogContent>
          <DialogActions className={classes.modalFooter}>
            <Button color="warning" size="sm" type="submit">
              {"Save"}
            </Button>
            <Button simple color="danger" onClick={() => setOpenModal(false)}>
              {"Cancel"}
            </Button>
          </DialogActions>{" "}
        </form>
      </Dialog>
    </>
  );
}
