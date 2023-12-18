<?php

namespace App\Models;

use Illuminate\Database\Eloquent\Factories\HasFactory;
use Illuminate\Database\Eloquent\Relations\HasMany;
use Illuminate\Database\Eloquent\Model;

class NXB extends Model
{
    protected $table = 'nxb'; 

    protected $fillable = [
        'tennxb', 
        'diachi',  
    ];

    public function sach()
    {
        return $this->belongsTo(Sach::class, 'sach_id', 'id');
    }

    
}

