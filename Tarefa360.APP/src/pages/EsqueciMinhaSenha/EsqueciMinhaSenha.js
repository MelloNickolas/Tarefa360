import style from './_esqueciMinhaSenha.module.css';
import Button from "react-bootstrap/Button";
import { useState } from "react";
import { useNavigate } from "react-router-dom";
import logo from '../../assets/LogoAzul.png';

export function EsqueciMinhaSenha(){

    
    return(

        <div className={style['pagina_conteudo']}>
            <div className={style['forgotPassword-container']}>
                <img src={logo} className={style['logo']}/>

                <div className={style['input-group']}>
                    <input type="email" placeholder="e-mail" />
                </div>
                
                <Button className={style['button_enviar']} type="submit"> Enviar </Button>

            </div>
        </div>
    );
}